using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        CancellationTokenSource cts = new CancellationTokenSource();
        protected void Page_Load(object sender, EventArgs e)
        {
            wordOccurance.Visible = false;
            wordCountTable.Visible = false;
        }

        void ReportProgress(int value)
        {
            //this.progressPanel.Attributes["aria-valuenow"] = value.ToString();
        }

        public List<KeyValuePair<string, int>> checkAsync(string filePath, IProgress<int> progress, CancellationToken ct)
        {
            Dictionary<string, int> contentCountDict = new Dictionary<string, int>();
            String fileData = File.ReadAllText(filePath);

            var contentCountList = new List<KeyValuePair<string, int>>();
            String[] fileWords = Regex.Split(fileData, @"\s");
            int totalFileWords = fileWords.Length;
            int totalWordsProcessed = 0;

            //sort the word count in eference with cont
            foreach (string fileWord in fileWords)
            {
                if (!string.IsNullOrWhiteSpace(fileWord))
                {
                    if (contentCountDict.ContainsKey(fileWord))
                    {
                        contentCountDict[fileWord] += 1;
                    }
                    else
                    {
                        contentCountDict.Add(fileWord, 1);
                    }
                    totalWordsProcessed++;
                    //cancel the file process
                    ct.ThrowIfCancellationRequested();
                    //update the progress
                    progress.Report(value: totalWordsProcessed * 100 / totalFileWords);
                    //Thread.Sleep(300);
                }
            }

            contentCountList = contentCountDict.ToList();
            contentCountList.Sort((content1, content2) => content1.Value.CompareTo(content2.Value));
            //Return the sorted word count list
            return contentCountList;
        }

        public async void ParseFile_click(object sender, EventArgs e)
        {
            wordOccurance.Visible = false;
            wordCountTable.Visible = false;
            if (uploadFile.HasFile)
            {
                String filePath = Server.MapPath("~/userFiles/") + uploadFile.FileName;
                //Save the uploaded file in userFiles folder
                var fileExt = Path.GetExtension(uploadFile.PostedFile.FileName);
                if (fileExt.Equals(".txt"))
                {
                    uploadFile.SaveAs(filePath);

                    Progress<int> progressIndicator = new Progress<int>(ReportProgress);
                    try
                    {
                        var contentCountList = await Task.Run(() => checkAsync(filePath, progressIndicator, cts.Token));
                        wordOccurance.Visible = true;
                        wordCountTable.Visible = true;
                        displayIfEmptyFile.InnerText = "";
                        //Append the sorted list to the table
                        foreach (var fileContentPair in contentCountList)
                        {
                            KeyValuePair<string, int> fileContent = fileContentPair;
                            TableRow row = new TableRow();
                            TableCell contentCell = new TableCell();
                            TableCell contentCountCell = new TableCell();
                            contentCell.Text = fileContent.Key;
                            row.Cells.Add(contentCell);
                            contentCountCell.Text = fileContent.Value.ToString();
                            row.Cells.Add(contentCountCell);
                            wordOccurance.Rows.Add(row);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //process cancelled
                    }
                }
                else
                {
                    displayIfEmptyFile.InnerText = "Please upload a text file";
                }
            }
            else
            {
                displayIfEmptyFile.InnerText = "Either the file is empty or their was some error processing";
            }
        }

        public void cancelParseFile_click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
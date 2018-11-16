<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        jQuery(document).ready(function () {
            $("#parseFile").on("click", function(){
                var uploadedFile = $("#uploadFile")[0].files;
                if (uploadedFile.length == 0) {
                alert("Please select a file!");
                }
            });
        });
    </script>
    <style>
        #wordOccurance{
            margin-top: 15px;
            border: solid thin black;
        }
        #wordOccurance th, #wordOccurance td{
            text-align: center;
            border: solid thin black;
            padding: 5px;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        
        <div style="margin: auto;width: 60%;padding: 10px;margin-top: 0px;">
            <fieldset>
                <legend>Add a text file to check the word count</legend>
                <div style="margin-bottom:5px;">
                    <asp:FileUpload ID="uploadFile" runat="server" Height="21px" />
                </div>
                <asp:Button ID="parseFile" runat="server" OnClick="ParseFile_click" Text="Click here" />
                <asp:Button ID="cancelFileParse" runat="server" OnClick="cancelParseFile_click" Text="Cancel" />
            </fieldset>
            <hr />
            <div runat="server" id="wordCountTable">
                <asp:Table ID="wordOccurance" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Word</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Occurence</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <span runat="server" id="displayIfEmptyFile"></span>
                   
        </div>
    </form>
    </body>
</html>

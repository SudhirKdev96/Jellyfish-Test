// opens raw PDF data in an iframe in a new tab
function OpenIntoNewTab(byteBase64) {
    var pdfWindow = window.open("");
    pdfWindow.document.write(
        "<iframe width='100%' height='100%' src='data:application/pdf;base64, " + byteBase64 + "'></iframe>"
    );
}

// triggers a download of a file from a byte array
function saveAsFile(filename, byteBase64) {
    var link = document.createElement('a');
    link.download = filename;
    link.href = "data:application/octet-stream;base64," + byteBase64;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
}
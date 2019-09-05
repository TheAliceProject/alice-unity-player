/**
* Resets TriLib browser files collection. Call it before adding new files to be loaded by TriLib.
* @param {number}	length	Number of files to be loaded.
*/
window.triLibResetFiles = function(length) {
  window.triLibFiles = [];
  window.triLibFilesCount = length;
  window.triLibFileIndex = 0;
};
/**
* Adds a new file to TriLib browser files collection. When all files have been loaded, TriLib native method "FilesLoaded" on "TriLibJsHelper" GameObject will be called.
* @param {string}		filename	Added file name.
* @param {Uint8Array}	filedata	Added file data.
*/
window.triLibAddFile = function(filename, filedata) {
  window.triLibFiles.push({
    name: filename,
    data: filedata
  });
  if (++window.triLibFileIndex >= window.triLibFilesCount) {
    unityInstance.SendMessage("TriLibJSHelper", "FilesLoaded", window.triLibFilesCount);
  }
};
/**
* Handles a paste event and sends the pasted text to Unity.
* @param {string}	text	Pasted text.
*/
window.triLibHandlePaste = function(text) {
  unityInstance.SendMessage("TriLibJSHelper", "OnPaste", text);
};
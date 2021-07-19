mergeInto(LibraryManager.library, {
  /**
   * Gets a file name from TriLib browser files collection by it's index.
   * @param {number}	index	File index.
   */
  TriLibGetBrowserFileName: function(index) {
    if (window.triLibFiles && window.triLibFiles[index]) {
      var file = window.triLibFiles[index];
      var name = file.name;
      var bufferSize = lengthBytesUTF8(name) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(name, buffer, bufferSize);
      return buffer;
    }
    return null;
  },
  /**
   * Gets a file data from TriLib browser files collection by it's index
   * @param {number}	index	File index.
   */
  TriLibGetBrowserFileData: function(index) {
    if (window.triLibFiles && window.triLibFiles[index]) {
      var file = window.triLibFiles[index];
      var data = file.data;
      var buffer = _malloc(data.length);
      writeArrayToMemory(data, buffer);
      return buffer;
    }
    return null;
  },
  /**
   * Gets a file length from TriLib browser files collection by it's index
   * @param {number}	index	File index.
   */
  TriLibGetBrowserFileLength: function(index) {
    if (window.triLibFiles && window.triLibFiles[index]) {
      var file = window.triLibFiles[index];
      var data = file.data;
      return data.length;
    }
    return 0;
  },
  /**
   * Deallocates native memory allocated by TriLib.
   * @param {number}	buffer	Buffer memory location.
   */
  TriLibFreeMemory: function(buffer) {
    if (buffer) {
      _free(buffer);
    }
  }
});

/*
Copyright (c) 2003-2012, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

	// 刪除圖片上傳功能用不到的分頁 ，刪除超連結用不到的分頁
	config.removeDialogTabs = 'image:Upload;image:advanced;image:Link;link:upload;link:advanced;link:target';


	/*	刪除輸入框下方計算字數功能: 於 ckeditor 資料夾 config.js 客製功能內加入以下程式碼*/
	config.removePlugins = 'elementspath';

};

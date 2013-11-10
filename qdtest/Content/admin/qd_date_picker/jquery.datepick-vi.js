/* http://keith-wood.name/datepick.html
   Vietnamese localisation for jQuery Datepicker.
   Translated by Le Thanh Huy (lthanhhuy@cit.ctu.edu.vn). */
(function($) {
	$.datepick.regional['vi'] = {
		monthNames: ['T.Một', 'T.Hai', 'T.Ba', 'T.Tư', 'T.Năm', 'T.Sáu',
		'T.Bảy', 'T.Tám', 'T.Chín', 'T.Mười', 'T.Mười Một', 'T.Mười Hai'],
		monthNamesShort: ['T.1', 'T.2', 'T.3', 'T.4', 'T.5', 'T.6',
		'T.7', 'T.8', 'T.9', 'T.10', 'T.11', 'T.12'],
		dayNames: ['Chủ Nhật', 'Thứ Hai', 'Thứ Ba', 'Thứ Tư', 'Thứ Năm', 'Thứ Sáu', 'Thứ Bảy'],
		dayNamesShort: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
		dayNamesMin: ['CN', 'T2', 'T3', 'T4', 'T5', 'T6', 'T7'],
		dateFormat: 'dd/mm/yyyy', firstDay: 0,
		renderer: $.datepick.defaultRenderer,
		prevText: '&#x3c;Trước', prevStatus: 'T.trước',
		prevJumpText: '&#x3c;&#x3c;', prevJumpStatus: 'Năm trước',
		nextText: 'Tiếp&#x3e;', nextStatus: 'T.sau',
		nextJumpText: '&#x3e;&#x3e;', nextJumpStatus: 'Năm sau',
		currentText: 'Hôm nay', currentStatus: 'T.hiện tại',
		todayText: 'Hôm nay', todayStatus: 'T.hiện tại',
		clearText: 'Xóa', clearStatus: 'Xóa ngày hiện tại',
		closeText: 'Đóng', closeStatus: 'Đóng và không lưu lại thay đổi',
		yearStatus: 'Năm khác', monthStatus: 'T.khác',
		weekText: 'Tu', weekStatus: 'Tuần trong năm',
		dayStatus: 'Đang chọn DD, \'ngày\' d M', defaultStatus: 'Chọn ngày',
		isRTL: false
	};
	$.datepick.setDefaults($.datepick.regional['vi']);
})(jQuery);

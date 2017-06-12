$(function () {
	
	//sidebar chose
	//$('.side_bar>li>a').click(function () {
	//	$('html, body').stop().animate({scrollTop:0}, '500', 'swing', function() { 
	//		alert("Finished animating");
	//	});
	//	$(this).parent().toggleClass("chosen");
	//	var filter_text = $.text([this]);
	//	var filter_no = $(".side_bar.filters>li").length;
	//	if ($(this).parent().hasClass("chosen")) {
	//		$(".side_bar.filters").append("<li class='filter_chosen chosen'><a href='#'>" + filter_text + "</a></li>");
	//	}
	//	else {
	//		$('.side_bar.filters>li').filter(function () { return $.text([this]) === filter_text }).remove();
	//	}

	//	CheckFiltersNo(filter_no);
	//});

	$('.side_bar>li>input[type="checkbox"]').on("change",function () {
		var filter_text="";
		$("label[for='" + this.id + "']").contents().each(function () {
			if (this.nodeType === 3) {
				filter_text += this.wholeText;
			}
		});
		var filter_no = $(".side_bar.filters>li").length;
		if ($(this).is(':checked')) {
			$(".side_bar.filters").append("<li class='filter_chosen chosen'><a href='#'>" + filter_text + "</a></li>");
			$('html, body').animate({scrollTop: 300}, 500);
		}
		else {
			$('.side_bar.filters>li').filter(function () { return $.text([this]) === filter_text }).remove();
		}

		CheckFiltersNo(filter_no);
	});

	$('body').on("click", '.side_bar.filters>li>a', (function () {
		var filter_no = $(".side_bar.filters>li").length;
		var filter_text = $.text([this]); //escape character .replace(/[!"#$%&'()*+,.\/:;<=>?@[\\\]^`{|}~]/g, "\\\\$&");
		$(this).parent().remove();
		$("input[id='" + $.trim(filter_text) + "']").click();
		//$('.side_bar>li').filter(function () { return $.text([this]) === filter_text }).toggleClass("chosen");
		CheckFiltersNo(filter_no);
	}));

	function CheckFiltersNo(filter_no) {
		if (($(".side_bar.filters>li").length == 1 && filter_no == 0) || ($(".side_bar.filters>li").length == 0 && filter_no == 1)) {
			$(".sidenav_header.filters").toggleClass("hidden");
			$(".side_bar.filters").toggleClass("hidden");
			$(".btn.btn-danger").toggleClass("hidden");
		}
	}
	//end sidebar chose
	//autocomplete
	$("#author").autocomplete({
		source: function (request, response) {
			$.ajax({
				url: '/Products/AutoComplete/',
				data: "{ 'prefix': '" + request.term + "'}",
				dataType: "json",
				type: "POST",
				contentType: "application/json; charset=utf-8",
				success: function (data) {
					response($.map(data, function (item) {
						return item;
					}))
				},
				error: function (response) {
					alert(response.responseText);
				},
				failure: function (response) {
					alert(response.responseText);
				}
			});
		},
		select: function (e, i) {
			$("#authorID").val(i.item.val);
		},
		minLength: 1
	});
	//end autocomplete
	//modal
	
	
		$(".books>a").animatedModal({
			modalTarget: 'modal-book',
			animatedIn: 'zoomIn',
			animatedOut: 'zoomOut',
			color: '#ffffff',
			// Callbacks
			beforeOpen: function () {
				console.log("ID html:");
			},
			afterOpen: function () {
				console.log(" ");
			},
			beforeClose: function () {
				$("#ajax-modal").html(" ");
			},
			afterClose: function () {
				console.log(" ");
			}
		});
 
		$(document).on("click", ".books>a", function () {
			$.ajax({
				url: "/Books/LoadModal",
				type: "POST",
				data: {bookId:$(this).prop('id')},
				success: function (data) {
					$("#ajax-modal").html(data);
				},
				error: function (data) {
					console.log("eroare");
				}

			});
	});
	//end modal
	//$('body').on('click', '.books>a', function (e) {
	//  e.stopPropagation();
	//});
		$(document).on("click", ".book_rent_button", function () {
			console.log("LOAN CLICKED");
		if ($('#registerLink').text() == "Register") {
			alert("You must be looged in to loan books! Register if you dont't have an account.")
		}
		else {
			$('html, body').animate({scrollTop: 300}, 500);
			$('.overlay').css({ 'opacity': 1, 'z-index': 9999 })
			$.ajax({
				url: "/Loans/Loan",
				type: "POST",
				data: { loanedId: this.getAttribute("loanedId") },
				success: function (data) {
					$("#ajax-result").html(data);
					$('.overlay').css({ 'opacity': 0, 'z-index': -9999 })
				},
				error: function (data) {
					console.log("eroare");
				}

			});
		}
	});
	$('.book_rent_button').on("click", function (event) {
		event.preventDefault()
		event.stopPropagation();
		if ($('#registerLink').text() == "Register") {
			alert("You must be looged in to loan books! Register if you dont't have an account.")
		} else {
			$('html, body').animate({ scrollTop: 300 }, 500);
			$('.overlay').css({ 'opacity': 1, 'z-index': 9999 })
			$.ajax({
				url: "/Loans/Loan",
				type: "POST",
				data: { loanedId: this.getAttribute("loanedId") },
				success: function (data) {
					$("#ajax-result").html(data);
					$('.overlay').css({ 'opacity': 0, 'z-index': -9999 })
				},
				error: function (data) {
					console.log("eroare");
				}

			});
		}
		return false;
	});
	$(".btn.btn-danger").on("click", function () {
		$('.side_bar>li>input[type="checkbox"]').prop("checked", false);
		$(".side_bar.filters").toggleClass('hidden');
		$(".side_bar.filters>li").remove()
		$('.sidenav_header.filters').toggleClass('hidden');
		$(this).toggleClass('hidden');
	});
	

	/*
	By Osvaldas Valutis, www.osvaldas.info
	Available for use under the MIT License
*/

	'use strict';

	; (function (document, window, index) {
		var inputs = document.querySelectorAll('.inputfile');
		Array.prototype.forEach.call(inputs, function (input) {
			var label = input.nextElementSibling,
				labelVal = label.innerHTML;

			input.addEventListener('change', function (e) {
				var fileName = '';
				if (this.files && this.files.length > 1)
					fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
				else
					fileName = e.target.value.split('\\').pop();

				if (fileName)
					label.querySelector('span').innerHTML = fileName;
				else
					label.innerHTML = labelVal;
			});

			// Firefox bug fix
			input.addEventListener('focus', function () { input.classList.add('has-focus'); });
			input.addEventListener('blur', function () { input.classList.remove('has-focus'); });
		});
	}(document, window, 0));
	//end input type file js
});
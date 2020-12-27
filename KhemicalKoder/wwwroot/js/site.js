// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function articles_search_clicked(searchString) {
    var searchBar = $('#articles_search_bar');
    var searchBarOptions = {
        container: 'body',
        html: true,
        trigger: 'manual',
        placement: 'bottom',
        content: ''
    };
    
    searchBar.on('focusout', _ => {
        searchBar.popover('dispose');
    });

    if (searchString == null || searchString.length <= 0) {
        searchBarOptions.content = 'Search text cannot be empty';

        searchBar.focus();

        var searchBarPopover = searchBar.popover(searchBarOptions);
        searchBarPopover.popover('show');

        return;
    }

    var searchBtn = $('#articles_search_btn');
    searchBtn.addClass("disabled");
    searchBar.addClass("disabled");

    var searchSpinner = $('#articles_search_spinner');
    searchSpinner.show();

    var baseURI = document.location.href.replace(document.location.pathname, '');
    var request = new XMLHttpRequest();
    request.open('GET', baseURI + '/articles/api/search/' + searchString);

    request.onload = function() {
        console.log(request.response);
        var result = JSON.parse(request.response);

        searchBarOptions.content = document.createElement('div');
      

        if (result.length <= 0) {
            searchBarOptions.content.innerText = "No search result found!";
            searchBarOptions.content.classList.add('text-secondary');

            /* This focusing is here so as to make sure search popover is dismissable when the content is a simple text 
             -----------------------------------------------------------
             dismissing focus can be very difficult
             */
            searchBar.focus();

        } else {
            searchBarOptions.content.classList.add("list-group");
            searchBarOptions.content.classList.add("list-group-flush");
            result.forEach(item => {
                var li = document.createElement('a');
                li.innerText = item["Title"];
                li.classList.add("list-group-item");
                li.classList.add("list-group-item-action");
                li.href = baseURI + "/articles/Index/" + item["Id"];

                searchBarOptions.content.appendChild(li);
            });
        }

        var searchBarPopover = searchBar.popover(searchBarOptions);

        searchBarPopover.popover('show');

        /* since this is the new addition, base on the content i bet it should be the new focusable */
        searchBarPopover.on('focusout', (event) => {
            event.stopPropagation();
            searchBar.popover('dispose');
        })
     
        searchSpinner.hide();
        searchBtn.removeClass("disabled");
    }

    request.onerror = function (message) {
        searchBarOptions.content = document.createElement('div');
        searchBarOptions.content.classList.add('text-danger');
        searchBarOptions.content.innerText = "Search error, try again!";

        searchBar.focus();

        var searchBarPopover = searchBar.popover(searchBarOptions);
        searchBarPopover.popover('show');

        searchSpinner.hide();
        searchBar.removeClass("disabled");
    }

    request.send();
}

function articleslist_search_clicked(searchString) {
    var baseURI = document.location.href.replace(document.location.pathname, '');
    baseURI = baseURI + '/articles/search/' + searchString;
    window.location.href = baseURI;
}

function manage_articles_preview_toggle() {
    if ($('#articles_preview_container').is(':hidden')) {
       
        $('#articles_preview_container').show('slow', () => {
            $('#articles_preview_title').html($('#manage_articles_title').val())
            $('#articles_preview_story').html($('#manage_articles_story').val())
        });

        $('#manage_articles_editor').hide('fast');
    } else {
        $('#manage_articles_editor').show('fast');
        $('#articles_preview_container').hide('slow');
    }
}

/* 
 * ------------------------------------------------------------------------ 
 *                        Service Worker - sw.js
 * ------------------------------------------------------------------------
 */

if ('serviceWorker' in navigator) {
    //navigator.serviceWorker.register('/sw.js', { scope: '/' });
}
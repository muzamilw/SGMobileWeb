/*=========================================================================================
    File Name: line.js
    Description: Morris line chart
    ----------------------------------------------------------------------------------------
    Item Name: Stack - Responsive Admin Theme
    Version: 3.2
    Author: PIXINVENT
    Author URL: http://www.themeforest.net/user/pixinvent
==========================================================================================*/

// Line chart
// ------------------------------
$(window).on("load", function(){

    //Morris.Line({
    //    element: 'line-followers',
    //    data: [{
    //        "year": "2010",
      
    //        "Followers": 62
    //    }, {
    //        "year": "2011",
         
    //        "Followers": 120
    //    }, {
    //        "year": "2012",
          
    //        "Followers": 80
    //    }, {
    //        "year": "2013",
         
    //        "Followers": 75
    //    }, {
    //        "year": "2014",
          
    //        "Followers": 100
    //    }, {
    //        "year": "2015",
           
    //        "Followers": 80
    //    }, {
    //        "year": "2016",
          
    //        "Followers": 90
    //    }],
    //    xkey: 'year',
    //    ykeys: ['Followers'],
    //    labels: ['Followers'],
    //    resize: true,
    //    smooth: false,
    //    pointSize: 3,
    //    pointStrokeColors:['#FF4558'],
    //    gridLineColor: '#e3e3e3',
    //    behaveLikeLine: true,
    //    numLines: 6,
    //    gridtextSize: 14,
    //    lineWidth: 3,
    //    hideHover: 'auto',
    //    lineColors: ['#FF4558']
    //});

    Morris.Line({
        element: 'line-followings',
        data: [{
            "year": "2010",

            "Followers": 62
        }, {
            "year": "2011",

            "Followers": 120
        }, {
            "year": "2012",

            "Followers": 80
        }, {
            "year": "2013",

            "Followers": 75
        }, {
            "year": "2014",

            "Followers": 100
        }, {
            "year": "2015",

            "Followers": 80
        }, {
            "year": "2016",

            "Followers": 90
        }],
        xkey: 'year',
        ykeys: ['Followers'],
        labels: ['Followers'],
        resize: true,
        smooth: false,
        pointSize: 3,
        pointStrokeColors: ['#FF4558'],
        gridLineColor: '#e3e3e3',
        behaveLikeLine: true,
        numLines: 6,
        gridtextSize: 14,
        lineWidth: 3,
        hideHover: 'auto',
        lineColors: ['#FF4558']
    });

    Morris.Line({
        element: 'line-engagement',
        data: [{
            "year": "2010",

            "Followers": 62
        }, {
            "year": "2011",

            "Followers": 120
        }, {
            "year": "2012",

            "Followers": 80
        }, {
            "year": "2013",

            "Followers": 75
        }, {
            "year": "2014",

            "Followers": 100
        }, {
            "year": "2015",

            "Followers": 80
        }, {
            "year": "2016",

            "Followers": 90
        }],
        xkey: 'year',
        ykeys: ['Followers'],
        labels: ['Followers'],
        resize: true,
        smooth: false,
        pointSize: 3,
        pointStrokeColors: ['#FF4558'],
        gridLineColor: '#e3e3e3',
        behaveLikeLine: true,
        numLines: 6,
        gridtextSize: 14,
        lineWidth: 3,
        hideHover: 'auto',
        lineColors: ['#FF4558']
    });

    Morris.Line({
        element: 'line-likes',
        data: [{
            "year": "2010",

            "Followers": 62
        }, {
            "year": "2011",

            "Followers": 120
        }, {
            "year": "2012",

            "Followers": 80
        }, {
            "year": "2013",

            "Followers": 75
        }, {
            "year": "2014",

            "Followers": 100
        }, {
            "year": "2015",

            "Followers": 80
        }, {
            "year": "2016",

            "Followers": 90
        }],
        xkey: 'year',
        ykeys: ['Followers'],
        labels: ['Followers'],
        resize: true,
        smooth: false,
        pointSize: 3,
        pointStrokeColors: ['#FF4558'],
        gridLineColor: '#e3e3e3',
        behaveLikeLine: true,
        numLines: 6,
        gridtextSize: 14,
        lineWidth: 3,
        hideHover: 'auto',
        lineColors: ['#FF4558']
    });


    Morris.Line({
        element: 'line-posting',
        data: [{
            "year": "2010",

            "Followers": 62
        }, {
            "year": "2011",

            "Followers": 120
        }, {
            "year": "2012",

            "Followers": 80
        }, {
            "year": "2013",

            "Followers": 75
        }, {
            "year": "2014",

            "Followers": 100
        }, {
            "year": "2015",

            "Followers": 80
        }, {
            "year": "2016",

            "Followers": 90
        }],
        xkey: 'year',
        ykeys: ['Followers'],
        labels: ['Followers'],
        resize: true,
        smooth: false,
        pointSize: 3,
        pointStrokeColors: ['#FF4558'],
        gridLineColor: '#e3e3e3',
        behaveLikeLine: true,
        numLines: 6,
        gridtextSize: 14,
        lineWidth: 3,
        hideHover: 'auto',
        lineColors: ['#FF4558']
    });
});
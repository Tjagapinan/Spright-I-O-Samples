(function () {
    "use strict";

    angular.module(APPNAME)
        .config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {

            $routeProvider.when('/blog/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/blogTab.html',
                controller: 'blogController',
                controllerAs: 'bc'
            }).when('/blog/:websiteId/create', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/content.html',
                controller: 'adminBlogsContentController',
                controllerAs: 'aBContent'
            }).when('/blog/:websiteId/edit/:adminBlogId', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/content.html',
                controller: 'adminBlogsContentController',
                controllerAs: 'aBContent'
            }).when('/blog/:websiteId/media/:adminBlogId', {
                templateUrl: '/Scripts/sabio/application/adminblogs/templates/media.html',
                controller: 'adminBlogsMediaController',
                controllerAs: 'aBMedia'
            }).when('/details/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/details.html',
                controller: 'detailsController',
                controllerAs: 'main'
            }).when('/content/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/contentTab.html',
                controller: 'contentController',
                controllerAs: 'cc'
            }).when('/content/create/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/contentCreateTab.html',
                controller: 'contentCreateController',
                controllerAs: 'ccc'
            }).when('/content/edit/:pagesId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/contentCreateTab.html',
                controller: 'contentCreateController',
                controllerAs: 'ccc'
            }).when('/inventory/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/inventoryTab.html',
                controller: 'inventoryController',
                controllerAs: 'ic'
            }).when('/navigation/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/navigationTab.html',
                controller: 'navigationController',
                controllerAs: 'nc'
            }).when('/schema/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/schemaTab.html',
                controller: 'schemaController',
                controllerAs: 'sc'
            }).when('/create', {
                templateUrl: '/Scripts/sabio/application/websites/templates/Create.html',
                controller: 'websiteController',
                controllerAs: 'webController'
            }).when('/list', { //index listing all websites
                templateUrl: '/Scripts/sabio/application/websites/templates/index.html',
                controller: 'listController',
                controllerAs: 'webController'
            }).when('/schema/:websiteId/entity/create', { //Create new schema for website
                templateUrl: '/Scripts/sabio/application/websites/templates/entityCreate.html',
                controller: 'evaEntityController',
                controllerAs: 'entity'
            }).when('/schema/:websiteId/entity/:entityId/edit', { //Create new schema for website
                templateUrl: '/Scripts/sabio/application/websites/templates/entityCreate.html',
                controller: 'evaEntityController',
                controllerAs: 'entity'
            }).when('/records/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/recordsIndexTab.html',
                controller: 'recordsIndexController',
                controllerAs: 'records'
            }).when('/records/:websiteId/records/:entityId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/recordsList.html',
                controller: 'recordsEntityController',
                controllerAs: 'rEntity'
            }).when('/records/:websiteId/records/:entityId/create', {
                templateUrl: '/Scripts/sabio/application/websites/templates/recordsCreate.html',
                controller: 'recordsManageController',
                controllerAs: 'rManage'
            }).when('/records/:websiteId/records/:entityId/edit/:recordId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/recordsCreate.html',
                controller: 'recordsManageController',
                controllerAs: 'rManage'
            }).when('/categories/:websiteId', {
                templateUrl: '/Scripts/sabio/application/websites/templates/categoryTree.html',
                controller: 'categoryTreeController',
                controllerAs: 'CTC'
            });;

            $locationProvider.html5Mode(false);

        }]);

})();
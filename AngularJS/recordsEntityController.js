(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('recordsEntityController', RecordsEntityController);

    RecordsEntityController.$inject = ['$scope', '$baseController', '$evaEntityService', '$evaAttributeService', '$evaRecordService'];

    function RecordsEntityController(
        $scope
        , $baseController
        , $evaEntityService
        , $evaAttributeService
        , $evaRecordService) {

        var vm = this;

        vm.items = null;
        vm.$evaEntityService = $evaEntityService;
        vm.$evaAttributeService = $evaAttributeService;
        vm.$evaRecordService = $evaRecordService;
        vm.currentPage = 1;
        vm.pageSize = 50;
        vm.totalItems = null;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$evaEntityService.getNotifier($scope);
        vm.notify = vm.$evaRecordService.getNotifier($scope);
        vm.websiteId = vm.$routeParams.websiteId;
        vm.entityId = vm.$routeParams.entityId;
        vm.recordId = vm.$routeParams.recordId;
        vm.pageChangeHandler = _pageChangeHandler;
        vm.deleteRecord = _deleteRecord;

        initialize();

        function initialize() {
            console.log("Records Entity controller initialized.");
            //$evaEntityService.loadRows(vm.websiteId, _getEntitySuccess, _getEntityError);
            //$evaRecordService.listRecsByEntId(vm.entityId, _getRecordSuccess, _getRecordError);
            var payload = {
                currentPage: vm.currentPage,
                itemsPerPage: vm.pageSize
            };
            $evaRecordService.ListRecordByEntity(vm.entityId, payload, _getRecordSuccess, _getRecordError);
        }

        function _getRecordSuccess(data) {
            console.log("Record get call = ", data.items);

            vm.notify(function () {
                vm.items = data.items;
                vm.entity = data.entity;
                vm.currentPage = data.currentPage;
                vm.pageSize = data.itemsPerPage;
                vm.totalItems = data.totalItems;

            });
        };

        function _getRecordError() {
            console.log("GET records error");
        };

        function _pageChangeHandler(currentPage) {
            vm.currentPage = currentPage;
            initialize();
        };

        function _deleteRecord(recordId) {
            $evaRecordService.DeleteRecordById(recordId, _deleteRecordSuccess, _deleteRecordError);
        };

        function _deleteRecordSuccess() {
            var payload = {
                currentPage: vm.currentPage,
                itemsPerPage: vm.pageSize
            };
            $evaRecordService.ListRecordByEntity(vm.entityId, payload, _getRecordSuccess, _getRecordError);
            console.log("successfully deleted")
        };

        function _deleteRecordError() {
            console.log("delete error");
        };
    }
})();
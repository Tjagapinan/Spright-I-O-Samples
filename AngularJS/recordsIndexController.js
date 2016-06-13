(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('recordsIndexController', RecordsIndexController);

    RecordsIndexController.$inject = ['$scope', '$baseController', '$evaEntityService'];

    function RecordsIndexController(
        $scope
        , $baseController
        , $evaEntityService) {

        var vm = this;

        vm.items = null;
        vm.$evaEntityService = $evaEntityService;


        $baseController.merge(vm, $baseController);

        vm.notify = vm.$evaEntityService.getNotifier($scope);

        vm.websiteId = vm.$routeParams.websiteId;
        vm.entityId = vm.$routeParams.entityId;
        render();




        function render() {
            $evaEntityService.loadRows(vm.websiteId, _renderEntitySuccess, _renderEntityError);

        }

        function _renderEntitySuccess(data) {
            vm.notify(function () {
                vm.items = data.items; //GET ACCESS ALL THE DATA FROM AJAX 

            });
        };

        function _renderEntityError() {
            console.log("unable to render entities.")
        };

        function _loadSchemaForm() {
            console.log("Load Schema form Button")
        };


    }
})();
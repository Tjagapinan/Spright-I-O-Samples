
(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('schemaController', SchemaController);

    SchemaController.$inject = ['$scope', '$baseController', '$evaEntityService'];

    function SchemaController(
        $scope
        , $baseController
        , $evaEntityService) {

        var vm = this;

        vm.items = null;
        vm.$evaEntityService = $evaEntityService;


        $baseController.merge(vm, $baseController);

        vm.notify = vm.$evaEntityService.getNotifier($scope);

        vm.websiteId = vm.$routeParams.websiteId;
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

    }
})();

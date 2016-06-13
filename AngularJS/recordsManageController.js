(function () {
    "use strict";

    angular.module(APPNAME)
        .controller('recordsManageController', RecordsManageController);

    RecordsManageController.$inject = ['$scope', '$baseController', '$evaEntityService', '$evaAttributeService', '$schemaFormService', '$evaRecordService', "$location"];

    function RecordsManageController(
        $scope
        , $baseController
        , $evaEntityService
        , $evaAttributeService
        , $schemaFormService
        , $evaRecordService
        , $location) {

        var vm = this;

        vm.schema = null;
        vm.form = null;
        vm.items = null;
        vm.model = {};
        vm.$evaEntityService = $evaEntityService;
        vm.$evaAttributeService = $evaAttributeService;
        vm.$schemaFormService = $schemaFormService;
        vm.$evaRecordService = $evaRecordService;

        $baseController.merge(vm, $baseController);

        vm.notify = vm.$schemaFormService.getNotifier($scope);

        vm.websiteId = vm.$routeParams.websiteId;
        vm.entityId = vm.$routeParams.entityId;
        vm.recordId = vm.$routeParams.recordId;
        vm.onSubmit = _onSubmit;

        vm.attributeArray = null; //attr array from schemasvc 
        vm.recordValueArray = [] //array of the saved record values
        vm.updateValue = null;

        initialize();

        function initialize() {
            

            if (vm.recordId) {
                console.log("This is an UPDATE");
                $evaRecordService.listValuesByRecordId(vm.recordId, _getValuesSuccess, _getValuesError);
                $evaEntityService.loadInfo(vm.entityId, _entitiesLoaded, _entityAjaxErr);
                //193; 
            }
            else {
                console.log("this is a NEW record");
                $evaEntityService.loadInfo(vm.entityId, _entitiesLoaded, _entityAjaxErr)
            }
        }

        function _getValuesSuccess(data) {
           
            vm.notify(function () {
                vm.model = vm.$schemaFormService.valuesToModel(data.item.value);
            });
        };

        function _getValuesError() {
            console.log("error on Values GET call");
        };


        function _entitiesLoaded(data) {
            //Convert data from ajax call to format that schemaForm can recognize
            var attrType = JSON.parse($("#attrType").html());

            vm.notify(function () {
                
                vm.schema = vm.$schemaFormService.entityToSchema(data.item, attrType);
                vm.form = vm.$schemaFormService.attributesToForm(data.item);

                vm.attributeArray = vm.$schemaFormService.attributeArray;
                
                
                if (vm.recordId) {
                    $evaRecordService.loadInfo(vm.entityId, _loadRecordValues, _checkEntityValuesError);
                }
            });
        };

        //error handler for entity ajax call
        function _entityAjaxErr() {
            console.log("Error loading entities");
        };

 
        //checkEntityValuesError
        function _checkEntityValuesError() {
            console.log("error checking entity values")
        };

        //submit handler for schemaForm
        function _onSubmit(form) {
            vm.$systemEventService.broadcast('schemaFormValidate');

            if (form.$valid) {
                console.log("form valid", vm.model);
                var attrArray = vm.attributeArray;

                var payload = {
                    EntityID: vm.entityId,
                    WebsiteId: vm.websiteId
                };

                payload.values = [];
                angular.forEach(attrArray[0], function (value, key) {

                    var valueObj = {};
                    valueObj.attributeID = value.id;

                    console.log("model data is ", vm.model[value.slug]);
                    switch (value.dataType) {
                        case 1:
                            valueObj.valueString = vm.model[value.slug];
                            break;
                        case 2:
                            valueObj.valueInt = vm.model[value.slug];
                            break;
                        case 3:
                            valueObj.valueDecimal = vm.model[value.slug];
                            break;
                        case 4:
                            valueObj.valueText = vm.model[value.slug];
                            break;
                        case 5:
                            valueObj.valueGeo = vm.model[value.slug];
                            break;
                    }

                    payload.values.push(valueObj);
                });

                console.log("payload is ", payload);
                console.log("record ID is = ", vm.recordId);
                if (vm.recordId) {

                    $evaRecordService.update(vm.recordId, payload, _onRecordUpdateSuccess, _onRecordUpdateError);
                    
                }
                else {
                    $evaRecordService.add(payload, _onRecordSubmitSuccess, _onRecordSubmitError);
                };
            } else {
                console.log("form not valid", vm.schemaForm);
            }

        };

        //success handler for submitting form
        function _onRecordSubmitSuccess() {
            console.log("Record has been sumbitted - POST");
            vm.$alertService.success("Record Submitted.");
            $location.path("/records/" + vm.websiteId + "/records/" + vm.entityId);
        };

        //error handler for submitting form
        function _onRecordSubmitError() {
            console.log("error on form submission - POST Error");
            vm.$alertService.error("Error");
        };

        //success handler for submitting form
        function _onRecordUpdateSuccess() {
            console.log("Record has been updated - PUT");
            vm.$alertService.success("Record Updated.");
            $location.path("/records/" + vm.websiteId + "/records/" + vm.entityId + "/edit/" + vm.recordId);
        };

        //error handler for submitting form
        function _onRecordUpdateError() {
            console.log("error on record update - PUT Error");
            vm.$alertService.error("Error");
        };
    }
})();


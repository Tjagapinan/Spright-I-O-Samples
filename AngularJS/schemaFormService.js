(function () {
    "use strict";

    angular.module(APPNAME)
        .factory('$schemaFormService', SchemaFormServiceFactory);

    SchemaFormServiceFactory.$inject = ['$baseService', '$sabio'];

    function SchemaFormServiceFactory($baseService, $sabio) {

        var svc = this;
        svc = $baseService.merge(true, {}, svc, $baseService);
        //functions used for converting data to schema
        svc.entityToSchema = _entityToSchema;
        svc.attributesToForm = _attributesToForm;
        svc.attributeArray = null;
        svc.valuesToModel = _valuesToModel;

        function _entityToSchema(entity, attrType) {
            //console.log("entity.name = ", entity, attrType)
            var schema = {
                type: "object",
                title: entity.name,
                properties: {},
                required: []
            };

            angular.forEach(entity.attributes, function (value, key) {
                //console.log("key is ", key);
                var type;

                for (var i in attrType) {
                    if (entity.attributes[key].dataType == i) {
                        type = attrType[i]
                        //console.log("type is ", attrType[i]);
                    }
                }

                switch (type) {
                    case "String":
                        type = "string";
                        break;
                    case "Text":
                        type = "string";
                        break;
                    default:
                        type = "number";
                }

                var myNewObject = { title: value.name, type: type }

                schema.properties[value.slug] = myNewObject;
            
                schema.required.push(value.slug);
            });

            
            return schema;

        };

        function _attributesToForm(entity) {
            var form = [];
            svc.attributeArray = [];
            svc.attributeArray.push(entity.attributes);
            
            angular.forEach(entity.attributes, function (value, key) {
                form.push(value.slug);
                
            });

            form.push({
                type: "submit",
                title: "save"
            });
            
            return form;
        };

        function _valuesToModel(attributes) {
            console.log("schema for values to model ", attributes);
            var model = {};
            for (var i = 0; i < attributes.length; i++) {

                var item = attributes[i];

                

                if (item.valueString != null) {
                    model[item.attributeSlug] = item.valueString;
                    
                } else if (item.valueInt > 0) {
                    model[item.attributeSlug] = item.valueInt;
                } else if (item.valueDecimal > 0) {
                    model[item.attributeSlug] = item.valueDecimal;
                } else if (item.valueText != null) {
                    model[item.attributeSlug] = item.valueText;
                } else if (item.valueGeo > 0) {
                    model[item.attributeSlug] = item.valueGeo;
                }

            };
            return model;

        };
        return svc;
    }
})();
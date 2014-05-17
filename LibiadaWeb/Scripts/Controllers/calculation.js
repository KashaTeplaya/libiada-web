﻿"use strict";

app.controller('CalculationCtrl', ['$scope', 'filterFilter', function ($scope, filterFilter) {

    MapModelFromJson($scope, data);

    $scope.characteristics = new Array();
    $scope.notationsFiltered = new Array();
    $scope.natureId = $scope.natures[0].Value;

    var filterByNature = function () {
        FilterOptionsByNature($scope, filterFilter, "notations");
        var notation = $scope.notationsFiltered[0];
        $scope.characteristics.forEach(function (characteristic) {
            characteristic.notation = notation;
        });

    };

    $scope.addCharacteristic = function () {
        $scope.characteristics.push({
            characteristicType: $scope.characteristicTypes[0],
            link: $scope.links[0],
            notation: $scope.notationsFiltered[0],
            language: $scope.languages[0],
            translator: $scope.translators[0]
        });
    };

    $scope.deleteCharacteristic = function (characteristic) {
        $scope.characteristics.splice($scope.characteristics.indexOf(characteristic), 1);
    };

    $scope.isLiteratureNature = function () {
        return $scope.natureLiterature == $scope.natureId;
    };

    $scope.isLinkable = function (characteristic) {
        return characteristic.characteristicType.Linkable;
    };

    $scope.$watch('natureId', filterByNature, true);
}]);
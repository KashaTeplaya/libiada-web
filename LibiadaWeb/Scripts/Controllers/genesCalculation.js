﻿"use strict";

angular.module("GenesCalculation", []).controller("GenesCalculationCtrl", ["$scope", genesCalculation]);

function genesCalculation($scope) {
    MapModelFromJson($scope, data);

    $scope.characteristics = new Array();

    $scope.addCharacteristic = function () {
        $scope.characteristics.push({
            characteristicType: $scope.characteristicTypes[0],
            link: $scope.links[0],
            notation: $scope.notations[0]
        });
    };

    $scope.deleteCharacteristic = function (characteristic) {
        $scope.characteristics.splice($scope.characteristics.indexOf(characteristic), 1);
    };

    $scope.isLinkable = function (characteristic) {
        return characteristic.characteristicType.Linkable;
    };
}

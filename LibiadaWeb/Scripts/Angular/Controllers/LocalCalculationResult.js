﻿function LocalCalculationResultController() {
	"use strict";

	function localCalculationResult($scope, $http) {

		function fillLegend() {
			$scope.legend = [];

			for (var k = 0; k < $scope.characteristics.length; k++) {
				$scope.legend.push({ name: $scope.characteristics[k].matterName, visible: true });
			}
		}

		// initializes data for chart
		function fillPoints() {
			$scope.points = [];
			var first = +$scope.firstCharacteristic.Value;
			var second = +$scope.secondCharacteristic.Value;

			for (var i = 0; i < $scope.characteristics.length; i++) {
				var characteristic = $scope.characteristics[i];
				for (var j = 0; j < characteristic.fragmentsData.length; j++) {
					var fragmentData = characteristic.fragmentsData[j];
					$scope.points.push({
						id: j,
						characteristicId: i,
						name: fragmentData.Name,
						x: fragmentData.Characteristics[first],
						y: fragmentData.Characteristics[second],
						cluster: characteristic.matterName
					});
				}
			}
		}

		// constructs string representing tooltip text (inner html)
		function fillPointTooltip(d) {
			var tooltipContent = [];
			tooltipContent.push(d.cluster);
			tooltipContent.push("Name: " + d.name);
			tooltipContent.push("Fragment №: " + d.id);
			var pointSharacteristics = [];
			var characteristics = $scope.characteristics[d.characteristicId].fragmentsData[d.id].Characteristics;
			for (var i = 0; i < characteristics.length; i++) {
				pointSharacteristics.push($scope.characteristicsList[i].Text + ": " + characteristics[i]);
			}

			tooltipContent.push(pointSharacteristics.join("<br/>"));

			return tooltipContent.join("</br>");
		}

		// shows tooltip for dot or group of dots
		function showTooltip(d, tooltip, svg) {
			$scope.clearTooltip(tooltip);

			tooltip.style("opacity", 0.9);

			var tooltipHtml = [];

			tooltip.selectedDots = svg.selectAll(".dot")
				.filter(function (dot) {
					if (dot.x === d.x && dot.y === d.y) {
						tooltipHtml.push($scope.fillPointTooltip(dot));
						return true;
					} else {
						return false;
					}
				})
				.attr("rx", $scope.selectedDotRadius)
				.attr("ry", $scope.selectedDotRadius);

			tooltip.html(tooltipHtml.join("</br></br>"));

			tooltip.style("background", "#eee")
				.style("color", "#000")
				.style("border-radius", "5px")
				.style("font-family", "monospace")
				.style("padding", "5px")
				.style("left", (d3.event.pageX + 10) + "px")
				.style("top", (d3.event.pageY - 8) + "px");

			tooltip.hideTooltip = false;
		}

		// clears tooltip and unselects dots
		function clearTooltip(tooltip) {
			if (tooltip) {
				if (tooltip.hideTooltip) {
					tooltip.html("").style("opacity", 0);

					if (tooltip.selectedDots) {
						tooltip.selectedDots.attr("rx", $scope.dotRadius)
							.attr("ry", $scope.dotRadius);
					}
				}

				tooltip.hideTooltip = true;
			}
		}

		function xValue(d) {
			return $scope.lineChart ? d.id : d.x;
		}

		function yValue(d) {
			return $scope.lineChart ? d.x : d.y;
        }

        function calculateLocalCharacteristicsSimilarityMatrix() {
            $http.get("/api/LocalCalculationWebApi?taskId=" + $scope.taskId
                    + "&aligner=" + $scope.aligner.Value
                    + "&distanceCalculator=" + $scope.distanceCalculator.Value
                    + "&aggregator=" + $scope.aggregator.Value)
                .then(function (result) {
                    $scope.comparisonMatrix = JSON.parse(result.data);
                }, function (error) {

                    alert("Failed loading alignment data");

                    $scope.loading = false;
                });
        }

        function draw() {
			$scope.fillPoints();

			// removing previous chart and tooltip if any
			d3.select(".tooltip").remove();
			d3.select(".chart-svg").remove();

			// chart size and margin settings
			var margin = { top: 30 + $scope.legendHeight, right: 30, bottom: 30, left: 60 };
			var width = $scope.width - margin.left - margin.right;
			var height = $scope.height - margin.top - margin.bottom;

			// setup x
			// calculating margins for dots
			var xMin = d3.min($scope.points, $scope.xValue);
			var xMax = d3.max($scope.points, $scope.xValue);
			var xMargin = (xMax - xMin) * 0.05;

			var xScale = d3.scaleLinear()
				.domain([xMin - xMargin, xMax + xMargin])
				.range([0, width]);
			var xAxis = d3.axisBottom(xScale)
				.tickSizeInner(-height)
				.tickSizeOuter(0)
				.tickPadding(10);

			$scope.xMap = function (d) { return xScale($scope.xValue(d)); };

			// setup y
			// calculating margins for dots
			var yMax = d3.max($scope.points, $scope.yValue);
			var yMin = d3.min($scope.points, $scope.yValue);
			var yMargin = (yMax - yMin) * 0.05;

			var yScale = d3.scaleLinear()
				.domain([yMin - yMargin, yMax + yMargin])
				.range([height, 0]);
			var yAxis = d3.axisLeft(yScale)
				.tickSizeInner(-width)
				.tickSizeOuter(0)
				.tickPadding(10);

			$scope.yMap = function (d) { return yScale($scope.yValue(d)); };

			// setup fill color
			var cValue = function (d) { return d.cluster; };
			var color = d3.scaleOrdinal(d3.schemeCategory20);
			var elementColor = function (d) { return color(cValue(d)); };

			// add the graph canvas to the body of the webpage
			var svg = d3.select("#chart").append("svg")
				.attr("width", $scope.width)
				.attr("height", $scope.height)
				.attr("class", "chart-svg")
				.append("g")
				.attr("transform", "translate(" + margin.left + "," + margin.top + ")");

			// add the tooltip area to the webpage
			var tooltip = d3.select("#chart").append("div")
				.attr("class", "tooltip")
				.style("opacity", 0);

			// preventing tooltip hiding if dot clicked
			tooltip.on("click", function () { tooltip.hideTooltip = false; });

			// hiding tooltip
			d3.select("#chart").on("click", function () { $scope.clearTooltip(tooltip); });

			// x-axis
			svg.append("g")
				.attr("class", "x axis")
				.attr("transform", "translate(0," + height + ")")
				.call(xAxis);

			svg.append("text")
				.attr("class", "label")
				.attr("transform", "translate(" + (width / 2) + " ," + (height + margin.top - $scope.legendHeight) + ")")
				.style("text-anchor", "middle")
				.text($scope.lineChart ? "Fragment №" : $scope.firstCharacteristic.Text)
				.style("font-size", "12pt");

			// y-axis
			svg.append("g")
				.attr("class", "y axis")
				.call(yAxis);

			svg.append("text")
				.attr("class", "label")
				.attr("transform", "rotate(-90)")
				.attr("y", 0 - margin.left)
				.attr("x", 0 - (height / 2))
				.attr("dy", ".71em")
				.style("text-anchor", "middle")
				.text($scope.lineChart ? $scope.firstCharacteristic.Text : $scope.secondCharacteristic.Text)
				.style("font-size", "12pt");

			if ($scope.lineChart) {
				var line = d3.line()
					.x($scope.xMap)
					.y($scope.yMap);

				// Nest the entries by symbol
				var dataNest = d3.nest()
					.key(function (d) { return d.cluster })
					.entries($scope.points);

				// Loop through each symbol / key
				dataNest.forEach(function (d) {
					svg.append("path")
						.datum(d.values)
						.attr("class", "line")
						.attr("d", line)
						.attr('stroke', function (d) { return color(cValue(d[0])); })
						.attr('stroke-width', 1)
						.attr('fill', 'none');
				});
			}
			// draw dots
			svg.selectAll(".dot")
				.data($scope.points)
				.enter()
				.append("ellipse")
				.attr("class", "dot")
				.attr("rx", $scope.dotRadius)
				.attr("ry", $scope.dotRadius)
				.attr("cx", $scope.xMap)
				.attr("cy", $scope.yMap)
				.style("fill-opacity", 0.6)
				.style("opacity", $scope.lineChart ? 0 : 1)
				.style("fill", elementColor)
				.style("stroke", elementColor)
				.on("click", function (d) { return $scope.showTooltip(d, tooltip, svg); });

			// draw legend
			var legend = svg.selectAll(".legend")
				.data($scope.legend)
				.enter()
				.append("g")
				.attr("class", "legend")
				.attr("transform", function (d, i) { return "translate(0," + i * 20 + ")"; })
				.on("click", function (d) {
					d.visible = !d.visible;
					var legendEntry = d3.select(this);
					legendEntry.select("text")
						.style("opacity", function () { return d.visible ? 1 : 0.5; });
					legendEntry.select("rect")
						.style("fill-opacity", function () { return d.visible ? 1 : 0; });

					svg.selectAll(".dot")
						.filter(function (dot) { return dot.cluster === d.name; })
						.attr("visibility", function (dot) {
							return d.visible ? "visible" : "hidden";
						});

					svg.selectAll(".line")
						.filter(function (line) { return line[0].cluster === d.name; })
						.attr("visibility", function (line) {
							return d.visible ? "visible" : "hidden";
						});
				});

			// draw legend colored rectangles
			legend.append("rect")
				.attr("width", 15)
				.attr("height", 15)
				.style("fill", function (d) { return color(d.name); })
				.style("stroke", function (d) { return color(d.name); })
				.style("stroke-width", 4)
				.attr("transform", "translate(0, -" + $scope.legendHeight + ")");

			// draw legend text
			legend.append("text")
				.attr("x", 24)
				.attr("y", 9)
				.attr("dy", ".35em")
				.attr("transform", "translate(0, -" + $scope.legendHeight + ")")
				.text(function (d) { return d.name; })
				.style("font-size", "9pt");
		}

        $scope.calculateLocalCharacteristicsSimilarityMatrix = calculateLocalCharacteristicsSimilarityMatrix;
		$scope.draw = draw;
		$scope.fillPoints = fillPoints;
		$scope.fillPointTooltip = fillPointTooltip;
		$scope.showTooltip = showTooltip;
		$scope.clearTooltip = clearTooltip;
		$scope.fillLegend = fillLegend;
		$scope.yValue = yValue;
		$scope.xValue = xValue;

		$scope.width = 800;
		$scope.dotRadius = 4;
		$scope.selectedDotRadius = $scope.dotRadius * 2;

		$scope.loadingScreenHeader = "Loading data";

		var location = window.location.href.split("/");
		$scope.taskId = location[location.length - 1];

		$http.get("/api/TaskManagerWebApi/" + $scope.taskId)
			.then(function (data) {
				MapModelFromJson($scope, JSON.parse(data.data));

				$scope.fillLegend();

				$scope.firstCharacteristic = $scope.characteristicsList[0];
				$scope.secondCharacteristic = $scope.characteristicsList.length > 1 ? $scope.characteristicsList[1] : $scope.characteristicsList[0];
                $scope.aligner = $scope.aligners[0];
                $scope.distanceCalculator = $scope.distanceCalculators[0];
                $scope.aggregator = $scope.aggregators[0];

				$scope.legendHeight = $scope.legend.length * 20;
				$scope.height = 800 + $scope.legendHeight;

				$scope.loading = false;
			}, function () {
				alert("Failed loading local characteristics data");
				$scope.loading = false;
			});
	}

    angular.module("libiada").controller("LocalCalculationResultCtrl", ["$scope", "$http", localCalculationResult]);
}
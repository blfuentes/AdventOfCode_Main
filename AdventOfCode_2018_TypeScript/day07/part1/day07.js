"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var fs = require("fs");
var path = require('path');
// let filepath = path.join(__dirname, "../test01_input.txt");
// let filepath = path.join(__dirname, "../test02_input.txt");
// let filepath = path.join(__dirname, "../test03_input.txt");
var filepath = path.join(__dirname, "../day07_input.txt");
var text = fs.readFileSync(filepath, "utf-8");
var lines = text.split("\r\n");
var Node_1 = require("../Node");
var nodeTree;
var nodeList = [];
var _loop_1 = function (line) {
    var lineParts = line.split(" ");
    var tmpNode = nodeList.find(function (_n) { return _n.element == lineParts[7]; });
    if (tmpNode == undefined) {
        tmpNode = new Node_1.Node(lineParts[7], undefined);
    }
    var tmpParent = nodeList.find(function (_n) { return _n.element == lineParts[1]; });
    if (tmpParent == undefined) {
        tmpParent = new Node_1.Node(lineParts[1], undefined);
        nodeTree = tmpParent;
        nodeList.push(tmpParent);
    }
    tmpNode.parentNodes.push(tmpParent);
    tmpParent.childNodes.push(tmpNode);
    tmpNode.parentNodes = tmpNode.parentNodes.sort(function (first, second) { return first.element.charCodeAt(0) - second.element.charCodeAt(0); });
    tmpParent.childNodes = tmpParent.childNodes.sort(function (first, second) { return first.element.charCodeAt(0) - second.element.charCodeAt(0); });
    if (nodeList.find(function (_n) { return _n.element == lineParts[7]; }) == undefined) {
        nodeList.push(tmpNode);
    }
};
for (var _i = 0, lines_1 = lines; _i < lines_1.length; _i++) {
    var line = lines_1[_i];
    _loop_1(line);
}
var firstNode = nodeList.find(function (_n) { return _n.parentNodes.length == 0; });
var lastNode = nodeList.find(function (_n) { return _n.childNodes.length == 0; });
function findNextElement(input) {
    return input.sort().shift();
}
var parentNodes = [];
var currentSolution = [];
var availableElements = [];
parentNodes = nodeList.filter(function (_n) { return _n.parentNodes.length == 0; });
var endNodes = nodeList.filter(function (_n) { return _n.childNodes.length == 0; });
if (parentNodes.length > 0) {
    for (var _a = 0, parentNodes_1 = parentNodes; _a < parentNodes_1.length; _a++) {
        var parent_1 = parentNodes_1[_a];
        availableElements.push(parent_1.element);
    }
    var _loop_2 = function () {
        var elementToRemove = findNextElement(availableElements);
        if (elementToRemove != undefined) {
            currentSolution.push(elementToRemove);
            var tmpNode = nodeList.find(function (_n) { return _n.element == elementToRemove; });
            if (tmpNode != undefined) {
                nodeIndex = nodeList.indexOf(tmpNode);
                if (nodeIndex > -1) {
                    nodeList.splice(nodeIndex, 1);
                    for (var _b = 0, _c = tmpNode.childNodes; _b < _c.length; _b++) {
                        var child = _c[_b];
                        if (currentSolution.indexOf(child.element) == -1) {
                            var existsParent = true;
                            for (var _d = 0, _e = child.parentNodes; _d < _e.length; _d++) {
                                var parent_2 = _e[_d];
                                if (currentSolution.indexOf(parent_2.element) == -1) {
                                    existsParent = false;
                                    break;
                                }
                            }
                            if (existsParent && availableElements.indexOf(child.element) == -1) {
                                availableElements.push(child.element);
                            }
                        }
                    }
                }
            }
        }
    };
    var nodeIndex;
    while (availableElements.length > 0) {
        _loop_2();
    }
}
var result;
if (firstNode != undefined && lastNode != undefined) {
    console.log("First node: ".concat(firstNode.element));
    console.log("Last node: ".concat(lastNode.element));
    console.log("Solution part 1: ".concat(currentSolution.toString().replace(RegExp(",", "g"), "")));
}

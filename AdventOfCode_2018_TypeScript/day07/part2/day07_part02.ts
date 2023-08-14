import { Node } from "../Node";

function by<T extends keyof U, U>(property: T): (a: U, b: U) => number {
    return (a, b) => {
        return a[property] < b[property] ? -1 : a[property] > b[property] ? 1 : 0;
    };
}

export class Day07Part02 {
    findNextElements(elements: Array<Node>, workers: Array<Node>) {
        return elements
            .sort(by("element"))
            .filter(e => workers.find(w => w != null && w.element == e.element) == undefined)
            .slice(0, workers.filter(w => w == null).length);
    }

    findNextWorker(input: Array<Node>) {
        return input.findIndex(w => w == null);
    }

    execute() {
        let fs = require("fs");
        let path = require('path');
        //let filepath = path.join(__dirname, "../test01_input.txt");
        // let filepath = path.join(__dirname, "../test02_input.txt");
        // let filepath = path.join(__dirname, "../test03_input.txt");
        let filepath = path.join(__dirname, "../day07_input.txt");
        let text: string = fs.readFileSync(filepath, "utf-8");
        let lines = text.split("\r\n");

        let nodeTree: Node | undefined;
        let nodeList: Array<Node> = []

        let baseDuration = 0;

        for (let line of lines) {
            let lineParts = line.split(" ");
            let tmpNode = nodeList.find(_n => _n.element == lineParts[7]);
            if (tmpNode == undefined) {
                tmpNode = new Node(lineParts[7], undefined, baseDuration);
            }
            let tmpParent = nodeList.find(_n => _n.element == lineParts[1]);
            if (tmpParent == undefined) {
                tmpParent = new Node(lineParts[1], undefined, baseDuration);
                nodeList.push(tmpParent);
            }
            tmpNode.parentNodes.push(tmpParent);
            tmpParent.childNodes.push(tmpNode);

            tmpNode.parentNodes = tmpNode.parentNodes.sort((first: Node, second: Node) => first.element.charCodeAt(0) - second.element.charCodeAt(0));
            tmpParent.childNodes = tmpParent.childNodes.sort((first: Node, second: Node) => first.element.charCodeAt(0) - second.element.charCodeAt(0));

            if (nodeList.find(_n => _n.element == lineParts[7]) == undefined) {
                nodeList.push(tmpNode);
            }
        }

        let currentSolution: Array<string> = [];
        let availableNodes: Array<Node> = [];
        availableNodes = nodeList.filter(_n => _n.parentNodes.length == 0);
        let runningSeconds = 60;
        let numberOfWorkers = 5;
        let workerQueue: Array<Node> = [];

        for (let idx = 0; idx < numberOfWorkers; idx++) {
            workerQueue[idx] = null;
        };

        while (availableNodes.length > 0) {
            console.log(`Second ${runningSeconds}`);
            let elementsForWorkers = this.findNextElements(availableNodes, workerQueue);
            for (let wIdx = 0; wIdx < numberOfWorkers; wIdx++) {
                if (workerQueue[wIdx] == null) {
                    // worker is free
                    if (elementsForWorkers.length > 0) {
                        let currentElement = elementsForWorkers.shift();
                        if (workerQueue.find(w => w != null && w.element == currentElement.element) == undefined) {
                            console.log(`Next element: ${currentElement.element}`);
                            console.log(`Next worker: ${wIdx + 1}`);
                            workerQueue[wIdx] = currentElement;                            
                        }
                    }
                }
            }
            workerQueue.forEach(w => {
                if (w != null) {
                    w.duration = w.duration - 1;
                    if (w.duration == 0) {
                        currentSolution.push(w.element);
                    }
                }
            });
            availableNodes = availableNodes.filter(n => n.duration != 0);
            for(let wIdx = 0; wIdx < numberOfWorkers; wIdx++) {
                let worker = workerQueue[wIdx];
                if (worker != null && worker.duration == 0) {
                    workerQueue[wIdx] = null;

                    let tmpNode = nodeList.find(node => node.element == worker.element);
                    var nodeIndex = nodeList.indexOf(tmpNode);
                    if (nodeIndex > -1) {
                        for (let child of tmpNode.childNodes) {
                            if (currentSolution.indexOf(child.element) == -1) {
                                let existsParent = true;
                                for (let parent of child.parentNodes) {
                                    if (currentSolution.indexOf(parent.element) == -1) {
                                        existsParent = false;
                                        break;
                                    }
                                }
                                if (existsParent && availableNodes.indexOf(child) == -1) {
                                    availableNodes.push(child);
                                }
                            }
                        }
                    }
                }
            }
            runningSeconds++;
        }
        return runningSeconds;
    }
}


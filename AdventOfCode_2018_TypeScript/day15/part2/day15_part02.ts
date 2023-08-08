import fs = require('fs');
import path = require('path');

import { Coord } from "../Coord";
import { ElementType, Element, Player, Elf, Goblin, Field, Wall } from "../Element";
import { MazeResult, MazePoint } from "../MazeResult";

export class Day15Part02 {
    displayCaveMap(lines, caveMap: Array<Array<string>>) {
        let rowIdx = 0;
        while (rowIdx < lines.length) {
            let rowDisplay = "";
            for (let colIdx = 0; colIdx < caveMap.length; colIdx++) {
                rowDisplay += caveMap[colIdx][rowIdx];
            }
            //console.log(`${rowDisplay}`);
            rowIdx++;
        }
    }

    AreEnemiesLeft(playerCollection, player: Player) {
        if (player.elementType == ElementType.ELF) {
            return playerCollection.find(_p => _p.elementType == ElementType.GOBLIN && _p.isAlive) != undefined;
        } else if (player.elementType == ElementType.GOBLIN) {
            return playerCollection.find(_p => _p.elementType == ElementType.ELF && _p.isAlive) != undefined;
        } else {
            return false;
        }
    }

    // 
    sortPlayerByPosition(a: Player, b: Player) {
        if (a.position.coordY == b.position.coordY) return a.position.coordX - b.position.coordX;
        return a.position.coordY - b.position.coordY;
    }
    sortByPosition(a: Coord, b: Coord) {
        if (a.coordY == b.coordY) return a.coordX - b.coordX;
        return a.coordY - b.coordY;
    }

    sortByArrayPosition(a: Array<Coord>, b: Array<Coord>) {
        if (a[1].coordY == b[1].coordY) return a[1].coordX - b[1].coordX;
        return a[1].coordY - b[1].coordY;
    }

    sortByMazePoint(a: MazePoint, b: MazePoint) {
        let aa = this.getNextStepMazePointResult(a);
        let bb = this.getNextStepMazePointResult(b);
        if (aa.coordY == bb.coordY) return aa.coordX - bb.coordX;
        return aa.coordY - bb.coordY;
    }

    getElementType(character: string) {
        if (character == "#") {
            return ElementType.WALL;
        } else if (character == "E") {
            return ElementType.ELF;
        } else if (character == "G") {
            return ElementType.GOBLIN;
        } else {
            return ElementType.FIELD;
        }
    }
    getTarget(mapPositions, player: Player) {
        let target: Player | undefined;
        let rangePositions = [
            [player.position.coordX, player.position.coordY - 1],
            [player.position.coordX - 1, player.position.coordY],
            [player.position.coordX + 1, player.position.coordY],
            [player.position.coordX, player.position.coordY + 1]
        ];
        let enemyType = ElementType.FIELD;
        if (player.elementType == ElementType.GOBLIN) {
            enemyType = ElementType.ELF;
        } else {
            enemyType = ElementType.GOBLIN;
        }
        let tmpCoord: Coord | undefined;
        let lowestHP = -1;
        for (let idx = 0; idx < rangePositions.length; idx++) {
            let pos = rangePositions[idx];
            tmpCoord = mapPositions.find(_m => _m.coordX == pos[0] && _m.coordY == pos[1] && _m.element.elementType == enemyType);
            if (tmpCoord != undefined) {
                if (lowestHP < 0) {
                    target = tmpCoord.element as Player;
                    lowestHP = target.HP;

                } else if ((tmpCoord.element as Player).HP < lowestHP) {
                    target = tmpCoord.element as Player;
                    lowestHP = target.HP;
                }
            }
        }
        return target;

    }
    findAvailableEnemyPositions(playerCollection, mapPositions, player: Player) {
        let enemies: Array<Player> = [];
        let enemyPositions: Array<Coord> = [];
        if (player.elementType == ElementType.ELF) {
            enemies = playerCollection.filter(_p => _p.elementType == ElementType.GOBLIN && _p.isAlive);
        } else if (player.elementType == ElementType.GOBLIN) {
            enemies = playerCollection.filter(_p => _p.elementType == ElementType.ELF && _p.isAlive);
        }
        for (let enemy of enemies) {
            // up
            let upCoord = mapPositions.find(_m => _m.coordX == enemy.position.coordX && _m.coordY == (enemy.position.coordY - 1));
            if (upCoord != undefined && upCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(upCoord);
            }
            // down
            let downCoord = mapPositions.find(_m => _m.coordX == enemy.position.coordX && _m.coordY == (enemy.position.coordY + 1));
            if (downCoord != undefined && downCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(downCoord);
            }
            // left
            let leftCoord = mapPositions.find(_m => _m.coordX == (enemy.position.coordX - 1) && _m.coordY == enemy.position.coordY);
            if (leftCoord != undefined && leftCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(leftCoord);
            }
            // right
            let rightCoord = mapPositions.find(_m => _m.coordX == (enemy.position.coordX + 1) && _m.coordY == enemy.position.coordY);
            if (rightCoord != undefined && rightCoord.element.elementType == ElementType.FIELD) {
                enemyPositions.push(rightCoord);
            }
        }
        return enemyPositions;
    }

    isValid(lines, mapPositions, visited: Array<Coord>, position: Coord) {
        let checkPosition = mapPositions.find(_p => _p.isEqual(position));
        if ((position.coordY >= 0) &&
            (position.coordY < lines.length) &&
            (position.coordX >= 0) &&
            (position.coordX < lines[0].length) &&
            (checkPosition != undefined && checkPosition.element.elementType == ElementType.FIELD)) {
            for (var j = 0; j < visited.length; j++) {
                if (visited[j].isEqual(position)) {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    move(lines, mapPositions, player: Player) {
        let coordToMove: Coord = new Coord(0, 0);
        let minDistance = 0;
        let doMove = false;
        player.EnemiesPositions = player.EnemiesPositions.sort(this.sortByPosition);
        for (let idx = 0; idx < player.EnemiesPositions.length; idx++) {
            let positionToCheck = player.EnemiesPositions[idx];
            let foundPath = this.findBFSPath(lines, mapPositions, player.position, positionToCheck);
            let tmpDistance = 0;
            if (foundPath) {
                tmpDistance = this.getLengthMazePointResult(foundPath);
                let tmpNextCoord = this.getNextStepMazePointResult(foundPath);
                if (idx == 0 || !doMove) {
                    doMove = true;
                    minDistance = tmpDistance;
                    if (tmpNextCoord != undefined) {
                        coordToMove = tmpNextCoord;
                    }
                } else if (tmpDistance < minDistance) {
                    minDistance = tmpDistance;
                    if (tmpNextCoord != undefined) {
                        coordToMove = tmpNextCoord;
                    }
                }
            }
        }

        if (doMove && coordToMove != undefined) {
            let newPosition = mapPositions.find(_p => _p.isEqual(coordToMove));
            if (newPosition != undefined) {
                player.NextPosition = newPosition;
            }
        }

    }

    getLengthMazePointResult(result: MazePoint) {
        let distance = 0;
        while (result.parent != null) {
            distance++;
            result = result.parent;
        }
        return distance;
    }

    getNextStepMazePointResult(result: MazePoint) {
        let invertedResult = Array<Coord>();
        invertedResult.push(result.position);
        while (result.parent != null) {
            invertedResult.push(result.position);
            result = result.parent;
        }
        invertedResult = invertedResult.reverse();
        return invertedResult.shift();
    }




    findBFSPath(lines, mapPositions, origin: Coord, target: Coord) {
        let resultPath = new Array<MazePoint>();
        let visistedMazePoints = new Array<Coord>();
        let tmpMazePoint = new MazePoint(origin, null);
        resultPath.push(tmpMazePoint);
        while (resultPath.length > 0) {
            let currentPoint = resultPath.shift();
            if (currentPoint != undefined &&
                currentPoint.position.isEqual(target)) {
                return currentPoint;
            }
            if (currentPoint != undefined &&
                visistedMazePoints.find(_v => _v.isEqual(currentPoint.position)) == undefined) {
                let neighbourMazePoint: MazePoint;
                let xCord: Array<number>;
                let yCord: Array<number>;
                xCord = [0, -1, 1, 0];
                yCord = [-1, 0, 0, 1];
                for (let idx = 0; idx < 4; idx++) {
                    neighbourMazePoint = new MazePoint(new Coord(currentPoint.position.coordX + xCord[idx], currentPoint.position.coordY + yCord[idx]), currentPoint);
                    if (this.isValid(lines, mapPositions, visistedMazePoints, neighbourMazePoint.position)) {
                        if (visistedMazePoints.find(_v => _v.isEqual(currentPoint.position)) == undefined) {
                            visistedMazePoints.push(currentPoint.position);
                        }
                        resultPath.push(neighbourMazePoint);
                    }
                }
            }
        }

        return null;
    }

    attack(mapPositions, caveMap, attacker: Player, target: Player) {
        if (target.HP > attacker.AP) {
            target.HP -= attacker.AP;
        } else {
            target.HP = 0;
            target.isAlive = false;

        }
        // console.log(`${target.display()} [${target.position.coordX}, ${target.position.coordY}] attacked by ` + 
        // `${attacker.display()} [${attacker.position.coordX}, ${attacker.position.coordY}]. ` +
        // `Damaged with ${attacker.AP} points. Current health: ${target.HP}.`)
        if (target.HP == 0) {
            let targetPosition = mapPositions.find(_p => _p.isEqual(target.position));
            if (targetPosition != undefined) {
                target.elementType = ElementType.FIELD;
                targetPosition.element = new Field(target.position);
                caveMap[targetPosition.coordX][targetPosition.coordY] = target.display();
            }
        }
    }

    round(lines, mapPositions, caveMap, player: Player) {
        let target = this.getTarget(mapPositions, player);
        if (target != undefined) {
            this.attack(mapPositions, caveMap, player, target);
        } else {
            this.move(lines, mapPositions, player);
            this.takeNewPlace(mapPositions, caveMap, player);
            target = this.getTarget(mapPositions, player);
            if (target != undefined) {
                this.attack(mapPositions, caveMap, player, target);
            }

        }
    }

    takeNewPlace(mapPositions, caveMap, player: Player) {
        if (player.NextPosition != undefined) {
            let newPosition = mapPositions.find(_p => _p.isEqual(player.NextPosition));
            let oldPosition = mapPositions.find(_p => _p.isEqual(player.position));
            if (oldPosition != undefined && newPosition != undefined) {
                oldPosition.isFree = true;
                oldPosition.element = new Field(new Coord(oldPosition.coordX, oldPosition.coordY));
                caveMap[oldPosition.coordX][oldPosition.coordY] = oldPosition.element.display();

                newPosition.isFree = false;
                newPosition.element = player;
                player.position = newPosition;
                caveMap[newPosition.coordX][newPosition.coordY] = newPosition.element.display();
            }
        }
    }



    initGame(lines, playerCollection, elfCollection, goblinCollection, mapPositions, AP: number) {
        let caveMap = Array(lines[0].length).fill(null).map(item => (new Array(lines.length).fill(" ")));

        playerCollection = [];
        elfCollection = [];
        goblinCollection = [];
        mapPositions = [];

        // READ MAP
        let rowIdx = 0;
        let cCard = 0;
        for (let line of lines) {
            let lineParts = line.split('');
            for (let column = 0; column < lineParts.length; column++) {
                let tmpmapelement = lineParts[column];
                let tmpelementtype = this.getElementType(tmpmapelement);
                let newCoord = new Coord(column, rowIdx);

                if (tmpelementtype == ElementType.FIELD) {
                    let newElement = new Field(newCoord);
                    newCoord.element = newElement;
                    newCoord.isFree = true;
                } else if (tmpelementtype == ElementType.WALL) {
                    let newElement = new Wall(newCoord);
                    newCoord.element = newElement;
                    newCoord.isFree = false;
                } else if (tmpelementtype == ElementType.ELF) {
                    let newElement = new Elf(newCoord);
                    newElement.isAlive = true;
                    newElement.AP = AP;
                    newCoord.element = newElement;
                    newCoord.isFree = false;

                    //
                    playerCollection.push(newElement);
                    elfCollection.push(newElement);
                } else if (tmpelementtype == ElementType.GOBLIN) {
                    let newElement = new Goblin(newCoord);
                    newElement.isAlive = true;
                    newCoord.element = newElement;
                    newCoord.isFree = false;

                    //
                    playerCollection.push(newElement);
                    goblinCollection.push(newElement);
                }
                caveMap[column][rowIdx] = lineParts[column];
                mapPositions.push(newCoord);
            }
            rowIdx++;
        }

        // sort units
        playerCollection = playerCollection.sort(this.sortPlayerByPosition);
    }


    execute() {
        // let filepath = path.join(__dirname, "../test_input.txt");
        // let filepath = path.join(__dirname, "../movement_input.txt");
        // let filepath = path.join(__dirname, "../basic_sample.txt");
        // let filepath = path.join(__dirname, "../combat_sample.txt");
        // let filepath = path.join(__dirname, "../combat_sample05.txt");
        let filepath = path.join(__dirname, "../day15_input.txt");
        let lines = fs.readFileSync(filepath, "utf-8").split("\r\n");
        // let lines = fs.readFileSync(filepath, "utf-8").split("\n");

        // INIT CAVEMAP
        let caveMap: Array<Array<string>> = [];
        caveMap = Array(lines[0].length).fill(null).map(item => (new Array(lines.length).fill(" ")));

        let playerCollection: Array<Player> = [];
        let elfCollection: Array<Elf> = [];
        let goblinCollection: Array<Goblin> = [];
        let mapPositions: Array<Coord> = [];
        let resultPath: Array<MazePoint>;
        let visistedMazePoints: Array<Coord>;

        let initAP = 2;
        let elfWasDead: Elf | undefined;
        do {
            this.initGame(lines, playerCollection, elfCollection, goblinCollection, mapPositions, ++initAP);
            //console.log(`Combat with ${initAP} AP.`);

            this.displayCaveMap(lines, caveMap);
            let roundNumber = 0;
            do {
                roundNumber++;
                //console.log(`Start of round ${roundNumber}.`);
                for (let player of playerCollection) {
                    if (player.isAlive && this.AreEnemiesLeft(playerCollection, player)) {
                        player.EnemiesPositions = this.findAvailableEnemyPositions(playerCollection, mapPositions, player).sort(this.sortByPosition);
                        this.round(lines, mapPositions, caveMap, player);
                    }
                }
                // displayCaveMap(caveMap);    
                //console.log(`End of round ${roundNumber}.`);
                playerCollection = playerCollection.filter(_p => _p.isAlive).sort(this.sortPlayerByPosition);

            } while (elfCollection.find(_e => _e.isAlive) && goblinCollection.find(_g => _g.isAlive))

            //
            let health = playerCollection.map(_p => _p.HP)
                .reduce((prev, curr) => prev + curr, 0)
            let score = health * (--roundNumber);

            //
            console.log(`finished! Score: ${score} (health: ${health}) (AP: ${initAP})`);

            elfWasDead = (elfCollection.find(_e => !_e.isAlive))
        } while (elfWasDead != undefined);

    }
}






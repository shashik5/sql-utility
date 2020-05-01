export const Utility = {
    isString(item): boolean {
        return Object.prototype.toString.call(item) === '[object String]';
    },
    isArray(item): boolean {
        return Object.prototype.toString.call(item) === '[object Array]';
    },
    pluck(list, propName): Array<string> {
        var propList = [];

        if (this.isArray(list)) {
            list.forEach(item => {
                propList.push(item[propName]);
            });
        };

        return propList;
    },
    unique(list): Array<string> {
        var uniqueList = [];

        if (this.isArray(list)) {
            list.forEach(item => {
                uniqueList.indexOf(item) == -1 && uniqueList.push(item);
            });
        };

        return uniqueList;
    }
}

export function DomHelper(element = null) {
    return new DomHelperClass(element);
}

export class DomHelperClass {
    public elements = [];

    constructor(element = null) {
        if (Utility.isArray(element)) {
            this.elements = element;
        }
        else if (Utility.isString(element)) {
            this.elements = window.document.querySelectorAll(element);
        }
        else {
            element && this.elements.push(element);
        };
    }

    private selectorType = {
        '.': 'hasClass',
        '#': 'hasId'
    };

    public parent(selector = ''): DomHelperClass {
        try {
            var parentEelment = DomHelper(),
                testElement = DomHelper(this.elements[0].parentNode);

            if (selector) {
                while (!testElement.isElementName('html')) {
                    if (testElement.is(selector)) {
                        parentEelment = testElement;
                        break;
                    };

                    testElement = DomHelper(testElement.elements[0].parentNode);
                };
            }
            else {
                parentEelment = testElement;
            };

            return parentEelment;
        } catch (e) {
            return DomHelper();
        };
    }

    public is(selector): boolean {
        try {
            var selectorType = selector[0],
                selectorText = selector.substr(1, selector.length - 1);

            if (this.selectorType.hasOwnProperty(selectorType)) {
                return this[this.selectorType[selectorType]](selectorText);
            }
            else {
                return this.isElementName(selector);
            };
        } catch (e) {
            return false;
        };
    }

    public hasClass(className): boolean {
        try {
            var result = false;

            this.elements.some(ele => {
                if (ele.classList.contains(className)) {
                    result = true;
                    return result;
                };
            });

            return result;
        } catch (e) {
            return false;
        };
    }

    public hasId(id): boolean {
        try {
            var result = false;

            this.elements.some(ele => {
                if (ele.id == id) {
                    result = true;
                    return true;
                };
            });

            return result;
        } catch (e) {
            return false;
        };
    }

    public isElementName(elementName): boolean {
        try {
            var result = false;

            this.elements.some(ele => {
                if (ele.nodeName.toLowerCase() == elementName.toLowerCase()) {
                    result = true;
                    return true;
                };
            });

            return result;
        } catch (e) {
            return false;
        };
    }

    public find(selector): DomHelperClass {
        try {
            var foundElements = [];
            this.elements.forEach(ele => {
                Array.prototype.push.apply(foundElements, ele.querySelectorAll(selector));
            });

            return DomHelper(foundElements);
        } catch (e) {
            return DomHelper();
        };
    }

    public indexOf(element): number {
        try {
            var index = -1;

            if (Utility.isArray(element.elements)) {
                element = element.elements[0];
            };

            this.elements.some((ele, idx) => {
                if (ele == element) {
                    index = idx;
                    return true;
                };
            });

            return index;
        } catch (e) {
            return -1;
        };
    }

    public addClass(className: string): DomHelperClass {
        try {
            var classNamesToBeAdded = className.split(' ');
            this.elements.forEach(ele => {
                classNamesToBeAdded.forEach(name => {
                    ele.classList.add(name);
                });
            });
        } catch (e) {

        } finally {
            return this;
        };
    }

    public removeClass(className: string): DomHelperClass {
        try {
            var classNamesToBeRemoved = className.split(' ');
            this.elements.forEach(ele => {
                classNamesToBeRemoved.forEach(name => {
                    ele.classList.remove(name);
                });
            });
        } catch (e) {

        } finally {
            return this;
        };
    }
}


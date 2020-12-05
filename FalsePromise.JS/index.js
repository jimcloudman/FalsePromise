import uuid from 'uuid-random';

export default class FalsePromise {

    constructor() {
        this.queue = [];
        window.falsePromise = {};

        const self = this;
        window.falsePromise.callback = function(id, error, message) {
            const match = self.queue.find(q => q.id === id);
            if (error) {
                if (match.error != null) {
                    match.error(error);
                }
            } else {
                if (match.then != null) {
                    match.then(message);
                }
            }
        };
    }

    execute(message, then, error) {
        const id = uuid();
        this.queue.push({id, then, error});
        window.chrome.webview.postMessage(JSON.stringify({
            Id: id, Message: JSON.stringify(message)
        }));
    }

    fetch(message) {
        return {
            then: thenFunc => {
                return {
                    error: errorFunc => {
                        return {
                            execute: () => this.execute(message, thenFunc, errorFunc)
                        }
                    },
                    execute: () => this.execute(message, thenFunc)
                };
            },
            error: errorFunc => {
                return {
                    then: thenFunc => {
                        return {
                            execute: () => this.execute(message, thenFunc, errorFunc)
                        };
                    },
                    execute: () => this.execute(message, null, errorFunc)
                };
            },
            execute: () => this.execute(message)
        }
    }
}
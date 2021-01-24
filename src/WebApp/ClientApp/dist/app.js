"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const React = require("react");
const ReactDOM = require("react-dom");
class App extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            auth: {
                token: "",
            }
        };
    }
    render() {
        return (React.createElement("div", null, "Hola !!!"));
    }
}
ReactDOM.render(React.createElement(React.StrictMode, null,
    React.createElement(App, null)), document.getElementById("app"));
//# sourceMappingURL=app.js.map
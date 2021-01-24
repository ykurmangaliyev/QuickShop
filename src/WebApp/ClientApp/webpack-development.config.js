const path = require("path");

module.exports = {
    mode: "development",

    devtool: "source-map",

    resolve: {
        extensions: [".ts", ".tsx", ".js"]
    },
    entry: "./app.tsx",
    output: {
        path: path.resolve(__dirname, "../wwwroot/dist/dev"),
        filename: "[name].js"
    },
    module: {
        rules: [
            {
                test: /\.ts(x?)$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: "ts-loader"
                    }
                ]
            },
            {
                enforce: "pre",
                test: /\.js$/,
                loader: "source-map-loader"
            }
        ]
    },
};
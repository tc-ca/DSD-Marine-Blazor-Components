const path = require('path');

module.exports = {
    entry: './configurable-date-input-polyfill.js',
    output: {
        filename: 'configurable-date-input-polyfill.dist.js',
        path: path.resolve(__dirname, ''),
        environment: {
            arrowFunction: false, // The environment supports arrow functions.
            bigIntLiteral: false, // The environment supports BigInt as literal.
            destructuring: false, // The environment supports destructuring.
            dynamicImport: false, // The environment supports an async import() function to import EcmaScript modules.
            module: false, // The environment supports ECMAScript Module syntax to import ECMAScript modules.
            const: false, // The environment supports const and let for variable declarations.
            forOf: false, // The environment supports 'for of' iteration.
        },
    },
    module: {
        rules: [
            {
                test: /\.m?js$/,
                exclude: /node_modules/,
                use: [{
                    loader: "babel-loader",
                    options: {
                        presets: ['@babel/preset-env']
                    }
                }, "eslint-loader"]
            },
            {
                test: /\.(s*)css$/,
                exclude: /node_modules/,
                use: ['style-loader', 'css-loader', 'sass-loader']
            }
        ]
    },
};
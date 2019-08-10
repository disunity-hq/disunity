const merge = require('webpack-merge');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = merge.smart(require('./webpack.config'), {
  mode: 'development',
  devtool: 'cheap-eval-source-map',
  output: {
    filename: 'js/[name].js'
  },
  plugins: [
    new MiniCssExtractPlugin({
      filename: '[name].css'
    })
  ],
});

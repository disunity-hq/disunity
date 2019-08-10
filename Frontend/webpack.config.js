const fs = require('fs');
const path = require('path');
const glob = require('glob');

const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CopyPlugin = require('copy-webpack-plugin');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

const entries = (() => {
  const pagesPath = path.join(__dirname, 'ts/pages');
  console.log(`pagesPath: ${pagesPath}`);
  var pages = glob.sync(`${pagesPath}/**/*.ts`);
  return pages.reduce(
    (acc, item) => {
      var relPath = path.relative(pagesPath, item);
      var keyName = relPath.replace(/\.[^/.]+$/, '');
      acc[keyName] = item;
      return acc;
    },
    {
      main: path.join(__dirname, 'ts/main.ts')
    }
  );
})();

for (let key in entries) {
  console.log(key);
}

const cssLoaders = [
  'css-loader',
  {
    loader: 'postcss-loader',
    options: {
      plugins: function() {
        return [require('autoprefixer')];
      }
    }
  },
  {
    loader: 'sass-loader',
    options: {
      includePaths: ['node_modules/@syncfusion']
    }
  }
];

module.exports = {
  entry: entries,
  output: {
    path: path.join(__dirname, 'dist'),
    libraryTarget: 'window'
  },
  module: {
    rules: [
      {
        test: /\.vue$/,
        loader: ['vue-loader', 'vue-inheritance-loader']
      },
      {
        test: /\.html$/,
        exclude: /node_modules/,
        loader: 'html-loader?exportAsEs6Default'
      },
      {
        test: /\.tsx?$/,
        loader: 'ts-loader',
        exclude: /node_modules/,
        options: {
          appendTsSuffixTo: [/\.vue$/]
        }
      },
      {
        test: /\.(css|scss|sass)$/,
        oneOf: [
          {
            resourceQuery: /^\?vue/,
            use: ['vue-style-loader', ...cssLoaders]
          },
          {
            use: [MiniCssExtractPlugin.loader, ...cssLoaders]
          }
        ]
      }
    ]
  },
  resolve: {
    extensions: ['.ts', '.tsx', '.js', '.jsx', '.html', '.vue'],
    alias: {
      vue$: 'vue/dist/vue.esm.js',
      shared: path.resolve(__dirname, 'ts/shared'),
      '@vendor': path.resolve(__dirname, 'ts/vendor')
    }
  },
  plugins: [
    new VueLoaderPlugin(),
    new CopyPlugin(['favicon.ico', { from: 'assets', to: 'assets' }])
  ]
};

import * as $ from 'jquery';
import { Driver } from './Scripts/Driver';

// Make Webpack pull in our CSS.
declare let require: any;
require('./Index.css');

// Globally install polyfills for Promise etc. (for IE11)
import 'core-js/shim';

// Start the application.
$(() => new Driver().drive());

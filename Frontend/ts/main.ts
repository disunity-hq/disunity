import 'bootstrap';
import * as jQuery from 'jquery';
import 'jquery-validation';
import 'jquery-validation-unobtrusive';
import 'timeago';
import { library, dom } from '@fortawesome/fontawesome-svg-core';
import { fas } from '@fortawesome/free-solid-svg-icons';
import '../css/main.scss';

jQuery(document).ready(() => {
  (jQuery('time.timeago') as any).timeago();
});

library.add(fas);
dom.watch();

export { jQuery };

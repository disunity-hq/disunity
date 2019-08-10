import { TemplateFunction, compile } from '@vendor/ejs';

import Template from './template.html';
import IErrorReport from '../IErrorReport';

export default class ArchiveFileValidationReport implements IErrorReport {
  private readonly template: TemplateFunction;

  constructor(private container: JQuery<HTMLElement>) {
    this.template = compile(Template);
  }

  ReportException(exception: any): void {
    const html = this.template(exception);

    this.container.empty();
    this.container.html(html);
  }
}

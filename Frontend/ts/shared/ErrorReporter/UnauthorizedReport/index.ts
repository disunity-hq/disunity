import * as ejs from '@vendor/ejs';


import Template from './template.html';
import IErrorReport from '../IErrorReport';


export default class UnauthorizedReport implements IErrorReport {

    private container: JQuery<HTMLElement>;
    private template: ejs.TemplateFunction;

    constructor(container: JQuery<HTMLElement>) {
        this.container = container;
        this.template = ejs.compile(Template);
    }

    public ReportException(exception: any) {
        var html = this.template(exception);
        this.container.empty();
        this.container.html(html);
    }

}
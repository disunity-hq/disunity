import * as ejs from '@vendor/ejs';
import { Grid, Sort } from '@syncfusion/ej2/grids';

Grid.Inject( Sort )

import Template from './template.html';


export default class ManifestSchemaReport {

    private container: JQuery<HTMLElement>;
    private template: ejs.TemplateFunction;

    constructor(container: JQuery<HTMLElement>) {
        this.container = container;
        this.template = ejs.compile(Template);
    }

    public ReportException(exception: any) {
        console.log("Manifest report sent exception");

        var html = this.template({
            grid_id: "error_grid"
        });

        this.container.empty();
        this.container.html(html);

        var grid = new Grid({
            allowSorting: true,
            dataSource: exception.errors,
            columns: [
                { field: 'kind', headerText: 'Error type', type: 'string' },
                { field: 'path', headerText: 'Field path', type: 'string' },
                { field: 'lineNumber', headerText: 'Line', type: 'number' },
                { field: 'linePosition', headerText: 'Column', type: 'string' },
                { field: 'message', headerText: 'Error message', type: 'string' },
            ],
            dataBound: () => grid.autoFitColumns(['kind', 'path', 'lineNumber', 'linePosition']),
            allowTextWrap: true,
            textWrapSettings: { wrapMode: 'Content' },            
        });

        grid.appendTo("#error_grid");
    }

}
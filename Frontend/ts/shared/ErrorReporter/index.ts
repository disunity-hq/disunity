import * as ejs from '@vendor/ejs';
import * as jQuery from 'jquery';
import ManifestSchemaReport from './ManifestSchemaReport';
import UnauthorizedReport from './UnauthorizedReport';
import IErrorReport from './IErrorReport';
import ArchiveFileValidationReport from './ArchiveFileValidationReport';

export default class ErrorReporter {
  private container: JQuery<HTMLElement>;
  private reportMap: Map<string, IErrorReport>;

  constructor(selector: string) {
    this.container = jQuery(selector);
    this.reportMap = new Map<string, IErrorReport>([
      ['Unauthorized', new UnauthorizedReport(this.container)],
      ['ManifestSchemaException', new ManifestSchemaReport(this.container)],
      [
        'ArchiveFormFileValidationError',
        new ArchiveFileValidationReport(this.container)
      ],
      ['ArchiveLoadException', new ArchiveFileValidationReport(this.container)]
    ]);
  }

  public Empty() {
    this.container.empty();
  }

  public Set(exception) {
    console.log('ErrorReporter sent exception');
    var reporter = this.reportMap.get(exception.type);

    if (reporter) {
      reporter.ReportException(exception);
    } else {
      console.log(`Unknown error type: ${exception.type}`);
    }
  }
}

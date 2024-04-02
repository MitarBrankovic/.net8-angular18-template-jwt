import { DatePipe } from '@angular/common';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'formatDate',
  standalone: true,
})
export class FormatDatePipe implements PipeTransform {
  transform(value: any, format: string = 'dd.MM.yyyy. HH:mm'): string | null {
    if (!value) return null;

    const datePipe = new DatePipe('en-US');
    return datePipe.transform(value, format);
  }
}

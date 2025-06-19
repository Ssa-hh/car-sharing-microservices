import { TemplateRef } from '@angular/core';

export interface Toast {
	type: 'info'|'success'|'error';
	message: string;
	classname?: string;
	delay?: number;
}
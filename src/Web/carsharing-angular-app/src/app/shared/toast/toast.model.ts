import { TemplateRef } from '@angular/core';

export interface Toast {
	type: 'info'|'success'|'error';
	message: string;
	title: string;
	classname?: string;
	delay?: number;
}
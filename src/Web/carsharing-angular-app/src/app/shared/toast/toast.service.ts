import { Injectable } from "@angular/core";
import {Toast} from './toast.model';


@Injectable({
  providedIn: 'root'
})
export class ToastService {
  	toasts: Toast[] = [];

  	private show(toast: Toast) {
		this.toasts.push(toast);
	}

	remove(toast: Toast) {
		this.toasts = this.toasts.filter((t) => t !== toast);
	}

	showStandard(message: string) {
		this.show({ type: 'info', message: message });
	}

	showSuccess(message: string) {
		this.show({ type: 'success', message: message, classname: 'bg-success text-light', delay: 10000 });
	}

	showDanger(message: string) {
		this.show({ type:'error', message: message, classname: 'bg-danger text-light', delay: 15000 });
	}
}

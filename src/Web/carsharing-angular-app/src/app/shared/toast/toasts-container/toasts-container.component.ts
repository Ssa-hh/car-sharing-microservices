import { Component, TemplateRef, ViewChild } from '@angular/core';
import { ToastService } from '../toast.service';

@Component({
  selector: 'app-toasts',
  standalone: false,
  templateUrl: './toasts-container.component.html',
  styleUrl: './toasts-container.component.css',
  host: { class: 'toast-container position-fixed top-0 end-0 p-3', style: 'z-index: 1200' }
})
export class ToastsContainerComponent {
  @ViewChild('standardTpl', {read: TemplateRef}) standardTemplate!: TemplateRef<any>;
  @ViewChild('successTpl', {read: TemplateRef}) successTemplate!: TemplateRef<any>;
  @ViewChild('dangerTpl', {read: TemplateRef}) dangerTemplate!: TemplateRef<any>;

  constructor(public readonly toastService: ToastService) {}

  GetTemplate(type: string): TemplateRef<any> {
    switch (type) {
      case 'success':
        return this.successTemplate;
      case 'error':
        return this.dangerTemplate;
      case 'info':
        return this.standardTemplate;
      default:
        return this.standardTemplate;
    }
  }
}

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateNamespaceComponent } from './create-namespace';

describe('CreateNamespaceComponent', () => {
  let component: CreateNamespaceComponent;
  let fixture: ComponentFixture<CreateNamespaceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateNamespaceComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateNamespaceComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CreateNamespaceFeature } from './create-namespace.feature';

describe('CreateNamespaceFeature', () => {
  let component: CreateNamespaceFeature;
  let fixture: ComponentFixture<CreateNamespaceFeature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateNamespaceFeature],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateNamespaceFeature);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

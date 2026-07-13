import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NamespaceContainerAssignmentFeature } from './namespace-container-assignment.feature';

describe('NamespaceContainerAssignmentFeature', () => {
  let component: NamespaceContainerAssignmentFeature;
  let fixture: ComponentFixture<NamespaceContainerAssignmentFeature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NamespaceContainerAssignmentFeature],
    }).compileComponents();

    fixture = TestBed.createComponent(NamespaceContainerAssignmentFeature);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

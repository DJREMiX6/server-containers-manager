import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NamespaceUserAssignmentFeature } from './namespace-user-assignment.feature';

describe('NamespaceUserAssignmentFeature', () => {
  let component: NamespaceUserAssignmentFeature;
  let fixture: ComponentFixture<NamespaceUserAssignmentFeature>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NamespaceUserAssignmentFeature],
    }).compileComponents();

    fixture = TestBed.createComponent(NamespaceUserAssignmentFeature);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

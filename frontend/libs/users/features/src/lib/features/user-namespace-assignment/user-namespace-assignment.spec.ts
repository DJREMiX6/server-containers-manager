import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserNamespaceAssignment } from './user-namespace-assignment';

describe('UserNamespaceAssignment', () => {
  let component: UserNamespaceAssignment;
  let fixture: ComponentFixture<UserNamespaceAssignment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UserNamespaceAssignment],
    }).compileComponents();

    fixture = TestBed.createComponent(UserNamespaceAssignment);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContainersListComponent } from './containers-list.component';

describe('ContainersListComponent', () => {
  let component: ContainersListComponent;
  let fixture: ComponentFixture<ContainersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainersListComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ContainersListComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContainersOverviewComponent } from './containers-overview.component';

describe('ContainersOverviewComponent', () => {
  let component: ContainersOverviewComponent;
  let fixture: ComponentFixture<ContainersOverviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainersOverviewComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ContainersOverviewComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

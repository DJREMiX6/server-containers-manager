import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ContainerOverviewCardComponent } from './container-overview-card.component';

describe('ContainerOverviewCardComponent', () => {
  let component: ContainerOverviewCardComponent;
  let fixture: ComponentFixture<ContainerOverviewCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ContainerOverviewCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(ContainerOverviewCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

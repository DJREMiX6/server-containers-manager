import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SideNavMenuComponent } from './side-nav-menu.component';

describe('SideNavMenuComponent', () => {
  let component: SideNavMenuComponent;
  let fixture: ComponentFixture<SideNavMenuComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideNavMenuComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SideNavMenuComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

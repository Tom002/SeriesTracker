import { observable, action, runInAction } from "mobx";
import { AuthService } from "../api/auth";
import { User } from "oidc-client";
import { IProfile } from "../models/profile";
import { createContext } from "react";
import agent from "../api/agent";
import { RootStore } from './rootStore';

class UserStore {
  authService = new AuthService();
  @observable currentUser: User | null = null;
  @observable userProfile: IProfile | null = null;

  rootStore: RootStore;

  constructor(rootStore: RootStore) {
    this.rootStore = rootStore;
  }

  @action loadUser = () => {
    this.authService.getUser().then((user) => {
      console.log('trying to load user');
      if (user) {
        runInAction(() => {
          this.currentUser = user;
          console.log('Current user after loading');
          console.log(this.currentUser);
        })
        
        agent.profile.get(user.profile.sub).then(
          (profile) => {
            if (profile) {
              runInAction(() => {
                this.userProfile = profile;
                console.log(profile);
              });
            }
          },
          (error) => {
            console.log(error);
          }
        );
      }
    });
  };
}

export default UserStore;

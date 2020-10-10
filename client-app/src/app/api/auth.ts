import { Log, User, UserManager, UserManagerSettings, WebStorageStateStore } from 'oidc-client';
import { Constants } from '../../helpers/Constants';

export class AuthService {
  public userManager: UserManager;

  constructor() {
    const settings : UserManagerSettings = {
      userStore: new WebStorageStateStore({ store: window.localStorage }),
      authority: Constants.identityServer,
      client_id: Constants.clientId,
      redirect_uri: `${Constants.clientRoot}signInCallback.html`,
      silent_redirect_uri: `${Constants.clientRoot}signInSilentCallback.html`,
      post_logout_redirect_uri: Constants.clientRoot,
      response_type: 'token id_token',
      scope: "openid users review gateway watching"
    };
    this.userManager = new UserManager(settings);

    Log.logger = console;
    Log.level = Log.INFO;
  }

  public getUser(): Promise<User | null> {
    return this.userManager.getUser();
  }

  public login(): Promise<void> {
    return this.userManager.signinRedirect();
  }

  public callback(): Promise<User> {
      return this.userManager.signinCallback();
  }
  
  public renewToken(): Promise<User> {
    return this.userManager.signinSilent();
  }

  public logout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }
}

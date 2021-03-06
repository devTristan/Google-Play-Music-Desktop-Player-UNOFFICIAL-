import { app } from 'electron';

Emitter.on('window:minimize', (event, windowID) => {
  WindowManager.getByInternalID(windowID).minimize();
});

Emitter.on('window:maximize', (event, windowID) => {
  const window = WindowManager.getByInternalID(windowID);
  if (window.isMaximized()) {
    WindowManager.getByInternalID(windowID).unmaximize();
  } else {
    WindowManager.getByInternalID(windowID).maximize();
  }
});

Emitter.on('window:close', (event, windowID) => {
  WindowManager.getByInternalID(windowID).close();
});

const mainWindow = WindowManager.getAll('main')[0];
mainWindow.on('close', (event) => {
  if (Settings.get('minToTray', true) && !global.quiting) {
    if (process.platform !== 'darwin') {
      mainWindow.minimize();
      mainWindow.setSkipTaskbar(true);
    } else {
      mainWindow.hide();
      if (app.dock && app.dock.hide) app.dock.hide();
    }
    event.preventDefault();
    return;
  }
});

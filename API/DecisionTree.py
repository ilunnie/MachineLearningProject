import pandas as pd

from joblib import dump

from sklearn.model_selection import train_test_split
from sklearn.tree import DecisionTreeRegressor
from sklearn.metrics import mean_absolute_error

data = pd.read_csv('GenerateCsv/colorScale.csv')
print(data)

X = data[['L']]
Y = data.drop('L', axis=1)

X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=.2, random_state=42)

model = DecisionTreeRegressor()
model.fit(X_train, Y_train)

Y_pred = model.predict(X_test)

mse = mean_absolute_error(Y_test, Y_pred)
print("Mean Absolute Error:", mse)

dump(model, 'Models/DecisionTree.pkl')
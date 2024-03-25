import pandas as pd

from joblib import dump

from sklearn.decomposition import PCA
from sklearn.linear_model import ElasticNet
from sklearn.metrics import mean_absolute_error
from sklearn.model_selection import train_test_split, GridSearchCV, cross_val_score

data = pd.read_csv('GenerateCsv/colorScale.csv')
data.dropna(inplace = True)

X = data[['L']]
Y = data.drop('L', axis=1)

scores = cross_val_score(ElasticNet(fit_intercept=True), X, Y, cv = 8)
print(scores)

pca = PCA(n_components=1)
pca.fit(X)
X = pca.transform(X)

X_train, X_test, Y_train, Y_test = train_test_split(X, Y, test_size=.2, random_state=42)

model = GridSearchCV(
    ElasticNet(fit_intercept=True),
    {
        'alpha': list(map(lambda x: x / 10, range(1, 10))),
        'l1_ratio': list(map(lambda x: x / 10, range(1, 10))),
    },
    n_jobs = 4
)

model.fit(X_train, Y_train)
model = model.best_estimator_

Y_pred = model.predict(X_test)
mse = mean_absolute_error(Y_test, Y_pred)

print("Mean Absolute Error:", mse)

dump(model, 'Models/ElasticNet.pkl')